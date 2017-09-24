using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeleBanel.Models;
using TeleBanel.Models.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBanel
{
    public class BotManager
    {
        #region Properties

        protected string LastWaitingQuery { get; set; } = string.Empty;
        protected string BotApiPassword { get; set; }
        public string BotApiKey { get; set; }
        public Dictionary<int, UserWrapper> Accounts { get; set; }
        public TelegramBotClient Bot { get; set; }
        public BotKeyboardCollection KeyboardCollection { get; set; }
        public User Me { get; set; }

        public IWebsiteMiddleware WebsiteManager { set; get; }
        public IJobMiddleware JobManager { set; get; }
        public IInboxMiddleware InboxManager { set; get; }

        #endregion

        #region Constructors

        protected BotManager()
        {
            Accounts = new Dictionary<int, UserWrapper>();
        }
        public BotManager(string apiKey, string botPassword, IWebsiteMiddleware websiteInfo) : this()
        {
            BotApiKey = apiKey;
            BotApiPassword = botPassword;
            KeyboardCollection = new BotKeyboardCollection(websiteInfo.Url);
            WebsiteManager = websiteInfo;
        }

        #endregion

        #region Methods

        public async void StartListening()
        {
            Bot = new TelegramBotClient(BotApiKey);
            Bot.StartReceiving();
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;

            Me = await Bot.GetMeAsync();
            Console.WriteLine($"{Me.Username} Connected.");
        }


        private async void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            LastWaitingQuery = e.CallbackQuery.Data.ToLower();
            var userId = e.CallbackQuery.From.Id;

            if (UserAuthenticated(e.CallbackQuery.From)) // user authenticated
            {
                if (LastWaitingQuery.StartsWith(InlinePrefixKeys.PortfolioKey))
                    GoNextPortfolioStep(e);
                else if (LastWaitingQuery.StartsWith(InlinePrefixKeys.AboutKey))
                    GoNextAboutStep(e);
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard);
                }
            }
            else // Before authenticate
                await AsPassword(e);
        }



        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var userId = e.Message.From.Id;
            var command = e.Message.Text?.GetNetMessage();

            if (e.Message.Chat.Type != ChatType.Private || e.Message.Type != MessageType.TextMessage ||
                string.IsNullOrEmpty(command))
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Localization.InvalidRequest);
                return;
            }
            

            if (command == Localization.GetMyId.ToLower())
            {
                await Bot.SendTextMessageAsync(userId, $"{e.Message.From.FirstName} {e.Message.From.LastName}, ID: {userId}");
            }
            else if (UserAuthenticated(e.Message.From)) // CommonReplyKeyboard
            {
                if (command == Localization.Portfolios.ToLower())
                {
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                        Localization.Portfolios,
                        replyMarkup: KeyboardCollection.PortfolioKeyboardInlineKeyboard);
                }
                else if (command == Localization.About.ToLower())
                {
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                        Localization.About + ": \n\r" + (WebsiteManager.About ?? "---"),
                        replyMarkup: KeyboardCollection.AboutKeyboardInlineKeyboard);
                }
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard);
                }
            }
            else // RegisterReplyKeyboard
            {
                if (command == Localization.Start.ToLower())
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard);
                }
                else if (command == Localization.Register.ToLower())
                {
                    Accounts[userId].Password = "";
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                        Localization.Password,
                        replyMarkup: KeyboardCollection.PasswordKeyboardInlineKeyboard);
                }
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard);
                }
            }
        }


        public async Task<bool> AsPassword(Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var userId = e.CallbackQuery.From.Id;

            if (!Accounts.ContainsKey(userId))
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                    Localization.EntryPasswordIsIncorrect,
                    showAlert: true);
                return false;
            }

            var data = e.CallbackQuery.Data.ToLower();
            if (!data.StartsWith("password_")) return false;

            if (data.StartsWith("password_num."))
            {
                Accounts[userId].Password += data.Replace("password_num.", "");
            }
            else if (data == "password_enter")
            {
                if (Accounts[userId].Password == BotApiPassword)
                {
                    Accounts[userId].IsAuthenticated = true;

                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.PasswordIsOk,
                        showAlert: true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard);
                    return true;
                }
                else // password is incorrect
                {
                    Accounts[userId].Password = "";
                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.EntryPasswordIsIncorrect,
                        showAlert: true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    return true;
                }
            }
            else if (data == "password_backspace")
            {
                if (Accounts[userId].Password.Length > 0)
                {
                    Accounts[userId].Password = Accounts[userId]
                            .Password.Remove(Accounts[userId].Password.Length - 1, 1);
                }
            }
            else
            {
                return false;
            }

            await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId,
                Localization.Password + ": " + new string(Accounts[userId].Password.Select(x => '*').ToArray()),
                ParseMode.Default, false, KeyboardCollection.PasswordKeyboardInlineKeyboard);

            return true;
        }
        public bool UserAuthenticated(User user)
        {
            if (!Accounts.ContainsKey(user.Id))
                Accounts[user.Id] = UserWrapper.Factory(user);

            return Accounts[user.Id].IsAuthenticated;
        }

        public async Task SendPhotoAsync(ChatId chatId, int userId, string caption, string imageName, byte[] imageBytes)
        {
            var msg = await Bot.SendTextMessageAsync(chatId, Localization.PleaseWait);

            using (var stream = new MemoryStream(imageBytes))
            {
                await Bot.SendPhotoAsync(chatId, new FileToSend(imageName, stream), caption);
            }

            await Bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
        }


        public async void GoNextPortfolioStep(Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var query = LastWaitingQuery.Replace(InlinePrefixKeys.PortfolioKey, "");

            switch (query)
            {
                case "addjob":
                    {
                        break;
                    }
                case "showjob":
                    {
                        var job = JobManager.GetJob(new Random().Next().ToString());
                        await SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.From.Id,
                            job.Title, job.Id, job.Image);
                        break;
                    }
                case "editjob":
                    {
                        break;
                    }
                case "deletejob":
                    {
                        break;
                    }
            }
        }

        private async void GoNextAboutStep(CallbackQueryEventArgs e)
        {
            var query = LastWaitingQuery.Replace(InlinePrefixKeys.AboutKey, "");
            if (query == "update")
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Please enter new About text", true);
            }
        }

        #endregion

    }
}