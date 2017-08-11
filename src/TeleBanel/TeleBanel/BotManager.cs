using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

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
        public IReplyMarkup VoteInlineKeyboard { get; set; }
        public IReplyMarkup PassKeyboardInlineKeyboard { get; set; }
        public IReplyMarkup CommonReplyKeyboard { get; set; }
        public IReplyMarkup RegisterReplyKeyboard { get; set; }

        #endregion

        #region Constructors

        protected BotManager()
        {
            CurrentCulture = new CultureInfo("fa");
            Accounts = new Dictionary<int, UserWrapper>();
            InitKeyboards();
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
            if (UserAuthenticated(e.CallbackQuery.From))
            {
                await Bot.SendTextMessageAsync(
                    e.CallbackQuery.Message.Chat.Id,
                    Localization.PleaseChooseYourOptionDoubleDot,
                    replyMarkup: CommonReplyKeyboard);
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
                            replyMarkup: CommonReplyKeyboard);
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
                            Localization.PleaseChooseYourOptionDoubleDot,
                            replyMarkup: RegisterReplyKeyboard);
                        break;
                    case "register":
                        Accounts[e.Message.From.Id].Password = "";
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.Password + ": ",
                            replyMarkup: PassKeyboardInlineKeyboard);
                        break;
                    case "change language":
                        Accounts[e.Message.From.Id].LanguageCulture = CurrentCulture.TwoLetterISOLanguageName;
                        InitKeyboards();
                        break;
                    case "get my id":
                        await Bot.SendTextMessageAsync(e.Message.From.Id,
                            $"{e.Message.From.FirstName} {e.Message.From.LastName} your id is: {e.Message.From.Id}");
                        break;
                    default:
                        return;
                }
            }
        }


        public async Task GetPasswordKeys(int userId)
        {
            if (!Accounts[userId].IsAuthenticated)
            {
                Accounts[userId].Password = "";
                await Bot.SendTextMessageAsync(userId,
                    Localization.Password + ": ",
                    ParseMode.Default,
                    replyMarkup: PassKeyboardInlineKeyboard);
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
            if (!Accounts.ContainsKey(e.CallbackQuery.From.Id))
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                    Localization.EntryPasswordIsIncorrect, showAlert: true);
                return false;
            }

            var data = e.CallbackQuery.Data.ToLower();

            if (data.StartsWith("num."))
            {
                Accounts[e.CallbackQuery.From.Id].Password += data.Replace("num.", "");
            }
            else if (data == "enter")
            {
                if (Accounts[e.CallbackQuery.From.Id].Password == BotApiPassword)
                {
                    Accounts[e.CallbackQuery.From.Id].IsAuthenticated = true;

                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.PasswordIsOk, showAlert: true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    return true;
                }
                else // password is incorrect
                {
                    Accounts[e.CallbackQuery.From.Id].Password = "";
                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        Localization.EntryPasswordIsIncorrect, true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    return true;
                }
            }
            else if (data == "backspace")
            {
                if (Accounts[e.CallbackQuery.From.Id].Password.Length > 0)
                {
                    Accounts[e.CallbackQuery.From.Id].Password =
                        Accounts[e.CallbackQuery.From.Id]
                            .Password.Remove(Accounts[e.CallbackQuery.From.Id].Password.Length - 1, 1);
                }
            }
            else
            {
                return false;
            }

            await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId,
                Localization.Password + ":" + new string(Accounts[e.CallbackQuery.From.Id].Password.Select(x => Emoji.EightPointedStar[0]).ToArray()),
                ParseMode.Default, false, PassKeyboardInlineKeyboard);

            return true;
        }


        public KeyboardButton[][] GetCommonReplyKeyboard()
        {
            var commonKeyboard = new[]
            {
                new[]
                {
                    new KeyboardButton("Edit Job \u270F"),
                    new KeyboardButton("Add Job"),
                    new KeyboardButton("Delete Job")
                },
                new[]
                {
                    new KeyboardButton("Edit Title"),
                    new KeyboardButton("Edit Footer"),
                    new KeyboardButton("Edit Logo")
                },
                new[]
                {
                    new KeyboardButton("Inbox"),
                    new KeyboardButton("Edit Social Links"),
                    new KeyboardButton("Edit About")
                },
                new[]
                {
                    new KeyboardButton("Change Language"),
                    new KeyboardButton("Help"),
                    new KeyboardButton("About")
                }
            };

            return commonKeyboard;
        }
        public KeyboardButton[] GetRegisterReplyKeyboard()
        {
            var keyboard = new[]
            {
                new KeyboardButton("Register"),
                new KeyboardButton("Get My ID"),
                new KeyboardButton("Change Language")
            };

            return keyboard;
        }

        public InlineKeyboardButton[][] GetVoteInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton("TESTs", "Alert"),
                    new InlineKeyboardCallbackButton("CallbackData", "NoAlert")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardSwitchInlineQueryButton("SwitchInlineQueryCurrentChat"),
                    new InlineKeyboardSwitchInlineQueryCurrentChatButton("SwitchInlineQuery", "/register")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardUrlButton("Url", "https://dezire.pro")
                }
            };

            return inlineKeys;
        }
        public InlineKeyboardButton[][] GetPassKeyboardInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap7, "Num.7"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap8, "Num.8"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap9, "Num.9")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap4, "Num.4"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap5, "Num.5"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap6, "Num.6")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap1, "Num.1"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap2, "Num.2"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap3, "Num.3")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.LeftArrow, "Backspace"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap0, "Num.0"),
                    new InlineKeyboardCallbackButton(Emoji.RightArrowCurvingLeft, "Enter")
                }
            };

            return inlineKeys;
        }

        public void InitKeyboards()
        {
            CommonReplyKeyboard = new ReplyKeyboardMarkup(GetCommonReplyKeyboard(), true);
            RegisterReplyKeyboard = new ReplyKeyboardMarkup(GetRegisterReplyKeyboard(), true);

            VoteInlineKeyboard = new InlineKeyboardMarkup(GetVoteInlineKeyboard());
            PassKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetPassKeyboardInlineKeyboard());
        }

        #endregion

    }
}
