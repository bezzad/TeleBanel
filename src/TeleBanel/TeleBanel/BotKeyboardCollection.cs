using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    public static class BotKeyboardCollection
    {
        public static Dictionary<LanguageCultures, IReplyMarkup> PassKeyboardInlineKeyboard { get; set; }
        public static Dictionary<LanguageCultures, IReplyMarkup> CommonReplyKeyboard { get; set; }
        public static Dictionary<LanguageCultures, IReplyMarkup> RegisterReplyKeyboard { get; set; }


        static BotKeyboardCollection()
        {
            CommonReplyKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();
            RegisterReplyKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();
            PassKeyboardInlineKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();

            foreach (LanguageCultures lang in Enum.GetValues(typeof(LanguageCultures)))
            {
                var culture = new CultureInfo(lang.ToString());
                CommonReplyKeyboard[lang] = new ReplyKeyboardMarkup(GetCommonReplyKeyboard(culture), true);
                RegisterReplyKeyboard[lang] = new ReplyKeyboardMarkup(GetRegisterReplyKeyboard(culture), true);
                PassKeyboardInlineKeyboard[lang] = new InlineKeyboardMarkup(GetPassKeyboardInlineKeyboard(culture));
            }

        }


        public static KeyboardButton[][] GetCommonReplyKeyboard(CultureInfo culture)
        {
            var commonKeyboard = new[]
            {
                new[]
                {
                    new KeyboardButton(Emoji.GreenApple + Localization.ResourceManager.GetString("Portfolio", culture)),
                    new KeyboardButton(Emoji.BallotBoxWithCheck + Localization.ResourceManager.GetString("Layout", culture)), // title, footer, about
                },
                new[]
                {
                    new KeyboardButton(Emoji.LadyBeetle + Localization.ResourceManager.GetString("Logo", culture)),
                    new KeyboardButton(Emoji.InboxTray + Localization.ResourceManager.GetString("Inbox", culture)),
                    new KeyboardButton(Emoji.Link + Localization.ResourceManager.GetString("SocialLinks", culture))
                },
                new[]
                {
                    new KeyboardButton(Localization.ResourceManager.GetString("ChangeLanguage", culture)),
                    new KeyboardButton(Localization.ResourceManager.GetString("About", culture))
                }
            };

            return commonKeyboard;
        }
        public static KeyboardButton[] GetRegisterReplyKeyboard(CultureInfo culture)
        {
            var keyboard = new[]
            {
                new KeyboardButton(Localization.ResourceManager.GetString("Register", culture)),
                new KeyboardButton(Localization.ResourceManager.GetString("GetMyId", culture)),
                new KeyboardButton(Localization.ResourceManager.GetString("ChangeLanguage", culture))
            };

            return keyboard;
        }
        public static InlineKeyboardButton[][] GetPassKeyboardInlineKeyboard(CultureInfo culture)
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
                    new InlineKeyboardCallbackButton(Localization.ResourceManager.GetString("Enter", culture), "Enter")
                }
            };

            return inlineKeys;
        }
    }
}