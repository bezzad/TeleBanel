using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TeleBanel.Helper;
using TeleBanel.Models;
using TeleBanel.Properties;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBanel
{
    // Bot Manager
    public partial class BotManager
    {
        public async Task StartListeningAsync()
        {
            Bot = new TelegramBotClient(BotApiKey);
            Bot.StartReceiving();
            Bot.OnMessage += BotOnMessage;
            Bot.OnCallbackQuery += BotOnCallbackQuery;
            Bot.OnReceiveError += BotOnReceiveError;

            Me = await Bot.GetMeAsync();
            Console.WriteLine(Localization.Bot_Connected, Me.Username);
        }
        
        public bool UserAuthenticated(User user, out UserWrapper userWrapper)
        {
            if (!Accounts.ContainsKey(user.Id))
                Accounts[user.Id] = UserWrapper.Factory(user);

            userWrapper = Accounts[user.Id];

            return Accounts[user.Id].IsAuthenticated;
        }
        
        private async void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            if (UserAuthenticated(e.CallbackQuery.From, out UserWrapper user)) // user authenticated
            {
                if (user.ConcurrencyController.CurrentCount == 0) return;
                try
                {
                    await user.ConcurrencyController.WaitAsync();
                    StartTimeToAnswerCallBack(user);
                    user.LastCallBackQuery = e.CallbackQuery;
                    var command = e.CallbackQuery.Data;
                    var methodName = command.Substring(0, command.IndexOf("_", StringComparison.Ordinal));
                    var method = GetType().GetMethod(PrefixKeys.CallbackQueryMethod + methodName, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (method != null)
                    {
                        await (Task)method.Invoke(this, new object[] { user });
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                    Debugger.Break();
                }
                finally
                {
                    user.ConcurrencyController.Release();
                }
            }
            else // Before authenticate
                await OnInlineKeyPassword(e);
        }

        private async void BotOnMessage(object sender, MessageEventArgs e)
        {
            var userId = e.Message.From.Id;
            var command = e.Message.Text?.GetNetMessage();

            if (e.Message.Chat.Type != ChatType.Private)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Localization.InvalidRequest);
                return;
            }

            if (UserAuthenticated(e.Message.From, out UserWrapper user)) // CommonReplyKeyboard
            {
                if (user.ConcurrencyController.CurrentCount == 0) return;
                try
                {
                    await user.ConcurrencyController.WaitAsync();
                    user.LastMessageQuery = e.Message;

                    var method = GetType()
                        .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                        .Where(m => m.Name.StartsWith(PrefixKeys.MessageMethod))
                        .FirstOrDefault(m => Localization.ResourceManager.GetString(m.Name.Replace(PrefixKeys.MessageMethod, "")) == command);

                    if (method != null)
                    {
                        await (Task)method.Invoke(this, new object[] { user });
                    }
                    else
                    {
                        if (user.WaitingMessageQuery != null)
                        {
                            var t = typeof(BotManager);
                            var m = t.GetMethod(user.WaitingMessageQuery);
                            if (m != null)
                                await (Task)m.Invoke(this, new object[] { user });
                        }
                        else
                            await OnMessageStart(user);
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                }
                finally
                {
                    user.ConcurrencyController.Release();
                }
            }
            else // RegisterReplyKeyboard
            {
                if (command == Localization.Register)
                    await OnMessageRegister(userId);
                else
                    await OnMessageBeforeRegister(userId);
            }
        }

        private void BotOnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            Console.WriteLine(e.ApiRequestException);
            Debugger.Break();
        }

    }
}