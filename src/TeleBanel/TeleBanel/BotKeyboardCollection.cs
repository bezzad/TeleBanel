using TeleBanel.Helper;
using TeleBanel.Properties;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    public class BotKeyboardCollection
    {
        public IReplyMarkup PasswordKeyboardInlineKeyboard { get; }
        public IReplyMarkup PortfolioKeyboardInlineKeyboard { get; }
        public IReplyMarkup AboutKeyboardInlineKeyboard { get; }
        public IReplyMarkup LogoKeyboardInlineKeyboard { get; }
        public IReplyMarkup CancelKeyboardInlineKeyboard { get; }
        public IReplyMarkup CommonReplyKeyboard { get; }
        public IReplyMarkup RegisterReplyKeyboard { get; }


        public BotKeyboardCollection(string url)
        {
            CommonReplyKeyboard = new ReplyKeyboardMarkup(GetCommonReplyKeyboard(), true);
            RegisterReplyKeyboard = new ReplyKeyboardMarkup(GetRegisterReplyKeyboard(), true);
            PasswordKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetPasswordKeyboardInlineKeyboard());
            PortfolioKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetPortfolioKeyboardInlineKeyboard(url));
            AboutKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetAboutKeyboardInlineKeyboard());
            LogoKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetLogoKeyboardInlineKeyboard());
            CancelKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetCancelKeyboardInlineKeyboard());
        }

        public KeyboardButton[][] GetCommonReplyKeyboard()
        {
            var commonKeyboard = new[]
            {
                new[]
                {
                    new KeyboardButton(Emoji.CardFileBox + " " + Localization.Portfolios),
                    new KeyboardButton(Emoji.ClosedMailboxWithLoweredFlag + " " + Localization.Inbox)
                },
                new[]
                {
                    new KeyboardButton(Emoji.Information + " " + Localization.About),
                    new KeyboardButton(Emoji.JapaneseSymbolForBeginner + " " + Localization.Logo),
                    new KeyboardButton(Emoji.Link + " " + Localization.SocialLinks)
                }
            };

            return commonKeyboard;
        }
        public KeyboardButton[] GetRegisterReplyKeyboard()
        {
            var keyboard = new[]
            {
                new KeyboardButton(Emoji.Key + " " + Localization.Register),
                new KeyboardButton(Emoji.IDButton + " " + Localization.GetMyId)
            };

            return keyboard;
        }

        public InlineKeyboardButton[][] GetPasswordKeyboardInlineKeyboard()
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
        public InlineKeyboardButton[][] GetPortfolioKeyboardInlineKeyboard(string url)
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.NEWButton + " " + Localization.AddJob, "Portfolio_AddJob"),
                    new InlineKeyboardCallbackButton(Emoji.Eye + " " + Localization.ShowJob, "Portfolio_ShowJob")

                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Crayon + " " +  Localization.EditJob, "Portfolio_EditJob"),
                    new InlineKeyboardCallbackButton(Emoji.CrossMark + " " +  Localization.DeleteJob, "Portfolio_DeleteJob")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardUrlButton(Emoji.Link + " " +  Localization.VisitWebsite, url)
                }
            };

            return inlineKeys;
        }
        public InlineKeyboardButton[][] GetAboutKeyboardInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Crayon + " " +  Localization.Update, "About_Update"),
                }
            };

            return inlineKeys;
        }
        public InlineKeyboardButton[][] GetLogoKeyboardInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.JapaneseSymbolForBeginner + " " +  Localization.ChangeLogo, "Logo_Update"),
                }
            };

            return inlineKeys;
        }
        public InlineKeyboardButton[][] GetCancelKeyboardInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.CrossMarkButton + " " +  Localization.Cancel, "Cancel"),
                }
            };

            return inlineKeys;
        }
    }
}