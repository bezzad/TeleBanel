using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    public static class BotKeyboardCollection
    {
        public static Dictionary<string, IReplyMarkup> VoteInlineKeyboard { get; set; }
        public static Dictionary<string, IReplyMarkup> PassKeyboardInlineKeyboard { get; set; }
        public static Dictionary<string, IReplyMarkup> CommonReplyKeyboard { get; set; }
        public static Dictionary<string, IReplyMarkup> RegisterReplyKeyboard { get; set; }


        static BotKeyboardCollection()
        {
            CommonReplyKeyboard = new Dictionary<string, IReplyMarkup>();
            RegisterReplyKeyboard = new Dictionary<string, IReplyMarkup>();
            VoteInlineKeyboard = new Dictionary<string, IReplyMarkup>();
            PassKeyboardInlineKeyboard = new Dictionary<string, IReplyMarkup>();

            foreach (LanguageCultures lang in Enum.GetValues(typeof(LanguageCultures)))
            {
                var l = lang.ToString().ToLower();
                CommonReplyKeyboard[l] = new ReplyKeyboardMarkup(GetCommonReplyKeyboard("en"), true);
                RegisterReplyKeyboard[l] = new ReplyKeyboardMarkup(GetRegisterReplyKeyboard("en"), true);
                VoteInlineKeyboard[l] = new InlineKeyboardMarkup(GetVoteInlineKeyboard("en"));
                PassKeyboardInlineKeyboard[l] = new InlineKeyboardMarkup(GetPassKeyboardInlineKeyboard("en"));
            }

        }


        public static KeyboardButton[][] GetCommonReplyKeyboard(string lang)
        {
            var commonKeyboard = new[]
            {
                new[]
                {
                    new KeyboardButton(Emoji.Pencil + "Edit Job"),
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
        public static KeyboardButton[] GetRegisterReplyKeyboard(string lang)
        {
            var keyboard = new[]
            {
                new KeyboardButton(Localization.Register),
                new KeyboardButton(Localization.GetMyId),
                new KeyboardButton(Localization.ChangeLanguage)
            };

            return keyboard;
        }
        public static InlineKeyboardButton[][] GetVoteInlineKeyboard(string lang)
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
        public static InlineKeyboardButton[][] GetPassKeyboardInlineKeyboard(string lang)
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
    }
}