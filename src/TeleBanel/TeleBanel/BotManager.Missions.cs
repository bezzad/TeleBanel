using System;
using System.Linq;
using System.Threading.Tasks;
using TeleBanel.Helper;
using TeleBanel.Models;
using TeleBanel.Properties;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TeleBanel
{
    public partial class BotManager
    {
        public async void GoNextPortfolioStep(UserWrapper user)
        {
            var query = user.LastCallBackQuery.Data.ToLower().Replace(InlinePrefixKeys.PortfolioKey, "");

            switch (query)
            {
                case "addjob":
                {
                    break;
                }
                case "showjob":
                {
                    var job = JobManager.GetJob(new Random().Next().ToString());
                    await Bot.SendPhotoAsync(user, job.Title, job.Id, job.Image);
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

        public async void GoNextAboutStep(UserWrapper user)
        {
            if (user.LastCallBackQuery == null)
                return;

            if (user.WaitingMessageQuery == null)
            {
                user.WaitingMessageQuery = nameof(GoNextAboutStep);
                await Bot.AnswerCallbackQueryAsync(user.LastCallBackQuery.Id, "Please enter new About and press Enter key.", true);
                await Bot.EditMessageReplyMarkupAsync(user.LastCallBackQuery.Message.Chat.Id,
                    user.LastCallBackQuery.Message.MessageId, KeyboardCollection.CancelKeyboardInlineKeyboard);
            }
            else
            {
                WebsiteManager.About = user.LastMessageQuery.Text;
                await Bot.AnswerCallbackQueryAsync(user.LastCallBackQuery.Id, "About successfully updated.", true);
                await Bot.EditMessageTextAsync(user.LastCallBackQuery.Message.Chat.Id, user.LastCallBackQuery.Message.MessageId,
                    Localization.About + ": \n\r" + (WebsiteManager.About ?? "---"),
                    ParseMode.Default, false, KeyboardCollection.AboutKeyboardInlineKeyboard);

                user.WaitingMessageQuery = null; // waiting method called and then clear buffer
            }
        }

        public async void GoNextLogoStep(UserWrapper user)
        {
            if (user.LastCallBackQuery == null)
                return;

            if (user.WaitingMessageQuery == null)
            {
                user.WaitingMessageQuery = nameof(GoNextAboutStep);
                await Bot.AnswerCallbackQueryAsync(user.LastCallBackQuery.Id, "Please enter new About and press Enter key.", true);
                await Bot.EditMessageReplyMarkupAsync(user.LastCallBackQuery.Message.Chat.Id,
                    user.LastCallBackQuery.Message.MessageId, KeyboardCollection.CancelKeyboardInlineKeyboard);
            }
            else
            {
                WebsiteManager.About = user.LastMessageQuery.Text;
                await Bot.AnswerCallbackQueryAsync(user.LastCallBackQuery.Id, "About successfully updated.", true);
                await Bot.EditMessageTextAsync(user.LastCallBackQuery.Message.Chat.Id, user.LastCallBackQuery.Message.MessageId,
                    Localization.About + ": \n\r" + (WebsiteManager.About ?? "---"),
                    ParseMode.Default, false, KeyboardCollection.AboutKeyboardInlineKeyboard);

                user.WaitingMessageQuery = null; // waiting method called and then clear buffer
            }
        }

        public async Task<bool> GoNextPasswordStep(CallbackQueryEventArgs e)
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
    }
}
