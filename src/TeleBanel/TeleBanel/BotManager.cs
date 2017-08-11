using System;
using System.Collections.Generic;
using System.Globalization;
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
        public string BotApiKey { get; set; }
        public Dictionary<int, UserWrapper> Accounts { get; set; }
        public TelegramBotClient Bot { get; set; }
        public User Me { get; set; }
        public CultureInfo CurrentCulture { get; set; }


        #endregion

        #region Constructors

        protected BotManager()
        {
            CurrentCulture = new CultureInfo(LanguageCultures.Fa.ToString().ToLower());
            Accounts = new Dictionary<int, UserWrapper>();
        }
        public BotManager(string apiKey, string botPassword) : this()
        {
            BotApiKey = apiKey;
            BotApiPassword = botPassword;
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
            Console.WriteLine("Bot loaded");
        }

        private void Bot_OnInlineResultChosen(object sender, Telegram.Bot.Args.ChosenInlineResultEventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void Bot_OnInlineQuery(object sender, Telegram.Bot.Args.InlineQueryEventArgs e)
        {
            if (string.IsNullOrEmpty(e.InlineQuery.Query)) return;

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

            if (UserAuthenticated(e.CallbackQuery.From))
            {
                await Bot.SendTextMessageAsync(
                    e.CallbackQuery.Message.Chat.Id,
                    Localization.PleaseChooseYourOptionDoubleDot,
                    replyMarkup: BotKeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
            }
            else
            {
                if (await AsPassword(e))
                {

                }
            }
        }
        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var userId = e.Message.From.Id;

            var command = e.Message.Text?.ToLower()?.Replace("/", "");

            if (e.Message.Chat.Type != ChatType.Private ||
                e.Message.Type != MessageType.TextMessage)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Localization.InvalidRequest);
                return;
            }


            var authenticated = UserAuthenticated(e.Message.From);

            if (authenticated)
            {
                switch (command)
                {
                    default:
                        await Bot.SendTextMessageAsync(
                            e.Message.Chat.Id,
                            Localization.PleaseChooseYourOptionDoubleDot,
                            replyMarkup: BotKeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case "start":
                        await Bot.SendTextMessageAsync(
                            e.Message.Chat.Id,
                            Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                            replyMarkup: BotKeyboardCollection.RegisterReplyKeyboard[Accounts[userId].LanguageCulture]);
                        break;
                    case "register":
                        Accounts[userId].Password = "";
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.ResourceManager.GetString("Password", Accounts[userId].Culture) + ": ",
                            replyMarkup: BotKeyboardCollection.PassKeyboardInlineKeyboard[Accounts[userId].LanguageCulture]);
                        break;
                    case "change language":
                        Accounts[userId].LanguageCulture = (LanguageCultures)Math.Abs((int)Accounts[userId].LanguageCulture - 1);
                        await Bot.SendTextMessageAsync(
                            e.Message.Chat.Id,
                            Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                            replyMarkup: BotKeyboardCollection.RegisterReplyKeyboard[Accounts[userId].LanguageCulture]);
                        break;
                    case "get my id":
                        await Bot.SendTextMessageAsync(userId,
                            $"{e.Message.From.FirstName} {e.Message.From.LastName} your id is: {userId}");
                        break;
                    default:
                        await Bot.SendTextMessageAsync(
                            e.Message.Chat.Id,
                            Localization.ResourceManager.GetString("PleaseChooseYourOptionDoubleDot", Accounts[userId].Culture),
                            replyMarkup: BotKeyboardCollection.RegisterReplyKeyboard[Accounts[userId].LanguageCulture]);
                        break;
                }
            }
        }

        public bool UserAuthenticated(Telegram.Bot.Types.User user)
        {
            if (!Accounts.ContainsKey(user.Id))
                Accounts[user.Id] = UserWrapper.Factory(user);

            return Accounts[user.Id].IsAuthenticated;
        }

        public async Task<bool> AsPassword(Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var userId = e.CallbackQuery.From.Id;

            if (!Accounts.ContainsKey(userId))
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                    Localization.EntryPasswordIsIncorrect, showAlert: true);
                return false;
            }

            var data = e.CallbackQuery.Data.ToLower();

            if (data.StartsWith("num."))
            {
                Accounts[userId].Password += data.Replace("num.", "");
            }
            else if (data == "enter")
            {
                if (Accounts[userId].Password == BotApiPassword)
                {
                    Accounts[userId].IsAuthenticated = true;

                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.PasswordIsOk, showAlert: true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: BotKeyboardCollection.CommonReplyKeyboard[Accounts[userId].LanguageCulture]);
                    return true;
                }
                else // password is incorrect
                {
                    Accounts[userId].Password = "";
                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.EntryPasswordIsIncorrect, true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    return true;
                }
            }
            else if (data == "backspace")
            {
                if (Accounts[userId].Password.Length > 0)
                {
                    Accounts[userId].Password =
                        Accounts[userId]
                            .Password.Remove(Accounts[userId].Password.Length - 1, 1);
                }
            }
            else
            {
                return false;
            }

            await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId,
                Localization.Password + ":" + new string(Accounts[userId].Password.Select(x => Emoji.EightPointedStar[0]).ToArray()),
                ParseMode.Default, false, BotKeyboardCollection.PassKeyboardInlineKeyboard[Accounts[userId].LanguageCulture]);

            return true;
        }


        #endregion

    }
}