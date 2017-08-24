using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBanel
{
    public class BotManager
    {
        #region Properties

        protected string BotApiPassword { get; set; }
        public string WebsiteUrl { get; set; }
        public string BotApiKey { get; set; }
        public Dictionary<int, UserWrapper> Accounts { get; set; }
        public TelegramBotClient Bot { get; set; }
        public BotKeyboardCollection KeyboardCollection { get; set; }
        public User Me { get; set; }
        public CultureInfo CurrentCulture { get; set; }

        public IJobManager JobManager { set; get; }

        #endregion

        #region Constructors

        protected BotManager()
        {
            CurrentCulture = new CultureInfo(LanguageCultures.Fa.ToString().ToLower());
            Accounts = new Dictionary<int, UserWrapper>();
        }
        public BotManager(string apiKey, string botPassword, string url) : this()
        {
            BotApiKey = apiKey;
            BotApiPassword = botPassword;
            WebsiteUrl = url;
            KeyboardCollection = new BotKeyboardCollection(url);
        }

        #endregion

        #region Methods

        public async void StartListening()
        {
            Bot = new TelegramBotClient(BotApiKey);
            Bot.StartReceiving();
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;
            Bot.OnInlineQuery += Bot_OnInlineQuery;
            Bot.OnInlineResultChosen += Bot_OnInlineResultChosen;

            Me = await Bot.GetMeAsync();
            Console.WriteLine($"{Me.Username} Connected.");
        }

        private void Bot_OnInlineResultChosen(object sender, Telegram.Bot.Args.ChosenInlineResultEventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void Bot_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        {
            //if (string.IsNullOrEmpty(e.InlineQuery.Query)) return;

            //InlineQueryResult[] results = {
            //    new InlineQueryResultLocation
            //    {
            //        Id = "1",
            //        Latitude = 40.7058334f, // displayed result
            //        Longitude = -74.25819f,
            //        Title = "New York",
            //        InputMessageContent = new InputLocationMessageContent // message if result is selected
            //        {
            //            Latitude = 40.7058334f,
            //            Longitude = -74.25819f,
            //        }
            //    },

            //    new InlineQueryResultLocation
            //    {
            //        Id = "2",
            //        Longitude = 52.507629f, // displayed result
            //        Latitude = 13.1449577f,
            //        Title = "Berlin",
            //        InputMessageContent = new InputLocationMessageContent // message if result is selected
            //        {
            //            Longitude = 52.507629f,
            //            Latitude = 13.1449577f
            //        }
            //    }
            //};

            //await Bot.AnswerInlineQueryAsync(e.InlineQuery.Id, results,
            //    isPersonal: true, cacheTime: 0);
        }
        private async void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var userId = e.CallbackQuery.From.Id;

            if (UserAuthenticated(e.CallbackQuery.From)) // user authenticated
            {
                var query = e.CallbackQuery.Data.ToLower();
                if (query.StartsWith(InlinePrefixKeys.PortfolioKey))
                    GoNextPortfolioStep(e);
                else if (query.StartsWith(InlinePrefixKeys.LayoutKey))
                    GoNextLayoutStep(e);
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
                }
            }
            else // Before authenticate
                await AsPassword(e);
        }
        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var userId = e.Message.From.Id;
            var command = e.Message.Text?.GetNetMessage();

            if (e.Message.Chat.Type != ChatType.Private ||
                e.Message.Type != MessageType.TextMessage ||
                string.IsNullOrEmpty(command))
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Localization.InvalidRequest);
                return;
            }

            var authenticated = UserAuthenticated(e.Message.From);

            if (authenticated) // CommonReplyKeyboard
            {
                if (command == Localization.ResourceManager.GetString("ChangeLanguage", Accounts[userId].Culture).ToLower())
                {
                    Accounts[userId].LanguageCulture = (LanguageCultures)Math.Abs((int)Accounts[userId].LanguageCulture - 1);
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
                }
                else if (command == Localization.ResourceManager.GetString("Portfolio", Accounts[userId].Culture).ToLower())
                {
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("Portfolio", Accounts[userId].Culture) + ": ",
                        replyMarkup: KeyboardCollection.PortfolioKeyboardInlineKeyboard[Accounts[userId].LanguageCulture]);
                }
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
                }
            }
            else // RegisterReplyKeyboard
            {
                if (command == Localization.ResourceManager.GetString("Start", Accounts[userId].Culture).ToLower())
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard[Accounts[userId].LanguageCulture]);
                }
                else if (command == Localization.ResourceManager.GetString("Register", Accounts[userId].Culture).ToLower())
                {
                    Accounts[userId].Password = "";
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("Password", Accounts[userId].Culture) + ": ",
                        replyMarkup: KeyboardCollection.PasswordKeyboardInlineKeyboard[Accounts[userId].LanguageCulture]);
                }
                else if (command == Localization.ResourceManager.GetString("GetMyId", Accounts[userId].Culture).ToLower())
                {
                    await Bot.SendTextMessageAsync(userId,
                        $"{e.Message.From.FirstName} {e.Message.From.LastName}, ID: {userId}");
                }
                else if (command == Localization.ResourceManager.GetString("ChangeLanguage", Accounts[userId].Culture).ToLower())
                {
                    Accounts[userId].LanguageCulture = (LanguageCultures)Math.Abs((int)Accounts[userId].LanguageCulture - 1);
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard[Accounts[userId].LanguageCulture]);
                }
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard[Accounts[userId].LanguageCulture]);
                }
            }
        }


        public async Task<bool> AsPassword(Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var userId = e.CallbackQuery.From.Id;

            if (!Accounts.ContainsKey(userId))
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                    Localization.ResourceManager.GetString("EntryPasswordIsIncorrect", Accounts[userId].Culture),
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
                        Localization.ResourceManager.GetString("PasswordIsOk", Accounts[userId].Culture),
                        showAlert: true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
                    return true;
                }
                else // password is incorrect
                {
                    Accounts[userId].Password = "";
                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.ResourceManager.GetString("EntryPasswordIsIncorrect", Accounts[userId].Culture),
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
                Localization.ResourceManager.GetString("Password", Accounts[userId].Culture) + ": " + new string(Accounts[userId].Password.Select(x => '*').ToArray()),
                ParseMode.Default, false, KeyboardCollection.PasswordKeyboardInlineKeyboard[Accounts[userId].LanguageCulture]);

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
            var msg = await Bot.SendTextMessageAsync(chatId,
                Localization.ResourceManager.GetString("PleaseWait", Accounts[userId].Culture));

            using (var stream = new MemoryStream(imageBytes))
            {
                await Bot.SendPhotoAsync(chatId, new FileToSend(imageName, stream), caption);
            }

            await Bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
        }

        public void GoNextLayoutStep(Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var query = e.CallbackQuery.Data.ToLower().Replace(InlinePrefixKeys.LayoutKey, "");

        }

        public async void GoNextPortfolioStep(Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var query = e.CallbackQuery.Data.ToLower().Replace(InlinePrefixKeys.PortfolioKey, "");

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
        #endregion

    }
}