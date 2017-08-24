using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    public class BotKeyboardCollection
    {
        public Dictionary<LanguageCultures, IReplyMarkup> PasswordKeyboardInlineKeyboard { get; set; }
        public Dictionary<LanguageCultures, IReplyMarkup> PortfolioKeyboardInlineKeyboard { get; set; }
        public Dictionary<LanguageCultures, IReplyMarkup> CommonReplyKeyboard { get; set; }
        public Dictionary<LanguageCultures, IReplyMarkup> RegisterReplyKeyboard { get; set; }


        public BotKeyboardCollection(string url)
        {
            CommonReplyKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();
            RegisterReplyKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();
            PasswordKeyboardInlineKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();
            PortfolioKeyboardInlineKeyboard = new Dictionary<LanguageCultures, IReplyMarkup>();

            foreach (LanguageCultures lang in Enum.GetValues(typeof(LanguageCultures)))
            {
                var culture = new CultureInfo(lang.ToString());
                CommonReplyKeyboard[lang] = new ReplyKeyboardMarkup(GetCommonReplyKeyboard(culture), true);
                RegisterReplyKeyboard[lang] = new ReplyKeyboardMarkup(GetRegisterReplyKeyboard(culture), true);
                PasswordKeyboardInlineKeyboard[lang] = new InlineKeyboardMarkup(GetPasswordKeyboardInlineKeyboard(culture));
                PortfolioKeyboardInlineKeyboard[lang] = new InlineKeyboardMarkup(GetPortfolioKeyboardInlineKeyboard(culture, url));
            }

        }

        public KeyboardButton[][] GetCommonReplyKeyboard(CultureInfo culture)
        {
            var commonKeyboard = new[]
            {
                new[]
                {
                    new KeyboardButton(Emoji.CardFileBox + " " + Localization.ResourceManager.GetString("Portfolio", culture)),
                    new KeyboardButton(Emoji.Gear + " " + Localization.ResourceManager.GetString("Layout", culture)) // title, footer, about
                },
                new[]
                {
                    new KeyboardButton(Emoji.LadyBeetle + " " + Localization.ResourceManager.GetString("Logo", culture)),
                    new KeyboardButton(Emoji.ClosedMailboxWithLoweredFlag + " " + Localization.ResourceManager.GetString("Inbox", culture)),
                    new KeyboardButton(Emoji.Link + " " + Localization.ResourceManager.GetString("SocialLinks", culture))
                },
                new[]
                {
                    new KeyboardButton(Emoji.Information + " " + Localization.ResourceManager.GetString("About", culture)),
                    new KeyboardButton(Emoji.ABButtonBloodType + " " + Localization.ResourceManager.GetString("ChangeLanguage", culture))
                }
            };

            return commonKeyboard;
        }
        public KeyboardButton[] GetRegisterReplyKeyboard(CultureInfo culture)
        {
            var keyboard = new[]
            {
                new KeyboardButton(Emoji.Key + " " + Localization.ResourceManager.GetString("Register", culture)),
                new KeyboardButton(Emoji.IDButton + " " + Localization.ResourceManager.GetString("GetMyId", culture)),
                new KeyboardButton(Emoji.ABButtonBloodType + " " + Localization.ResourceManager.GetString("ChangeLanguage", culture))
            };

            return keyboard;
        }
        
        public InlineKeyboardButton[][] GetPasswordKeyboardInlineKeyboard(CultureInfo culture)
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap7, "Password_Num.7"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap8, "Password_Num.8"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap9, "Password_Num.9")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap4, "Password_Num.4"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap5, "Password_Num.5"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap6, "Password_Num.6")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap1, "Password_Num.1"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap2, "Password_Num.2"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap3, "Password_Num.3")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.LeftArrow, "Password_Backspace"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap0, "Password_Num.0"),
                    new InlineKeyboardCallbackButton(Emoji.OKButton, "Password_Enter")
                }
            };

            return inlineKeys;
        }
        public InlineKeyboardButton[][] GetPortfolioKeyboardInlineKeyboard(CultureInfo culture, string url)
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.HeavyPlusSign + " " + Localization.ResourceManager.GetString("AddJob", culture), "Portfolio_AddJob"),
                    new InlineKeyboardCallbackButton(Emoji.Eye + " " + Localization.ResourceManager.GetString("ShowJob", culture), "Portfolio_ShowJob")
                    
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.HeavyCheckMark + " " +  Localization.ResourceManager.GetString("EditJob", culture), "Portfolio_EditJob"),
                    new InlineKeyboardCallbackButton(Emoji.HeavyMultiplicationX + " " +  Localization.ResourceManager.GetString("DeleteJob", culture), "Portfolio_DeleteJob")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardUrlButton(Emoji.Link + " " +  Localization.ResourceManager.GetString("VisitWebsite", culture), url)
                }
            };

            return inlineKeys;
        }
    }
}