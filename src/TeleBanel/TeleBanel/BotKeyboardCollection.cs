using TeleBanel.Helper;
using TeleBanel.Models.Middlewares;
using TeleBanel.Properties;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    public class BotKeyboardCollection
    {
        private IWebsiteMiddleware Website { get; set; }

        public IReplyMarkup PasswordKeyboardInlineKeyboard { get; }
        public IReplyMarkup PortfolioKeyboardInlineKeyboard { get; }
        public IReplyMarkup LinksboardInlineKeyboard => new InlineKeyboardMarkup(GetLinksKeyboardInlineKeyboard(Website));
        public IReplyMarkup AboutKeyboardInlineKeyboard { get; }
        public IReplyMarkup LogoKeyboardInlineKeyboard { get; }
        public IReplyMarkup CancelKeyboardInlineKeyboard { get; }
        public IReplyMarkup CommonReplyKeyboard { get; }
        public IReplyMarkup RegisterReplyKeyboard { get; }


        public BotKeyboardCollection(IWebsiteMiddleware website)
        {
            Website = website;

            CommonReplyKeyboard = new ReplyKeyboardMarkup(GetCommonReplyKeyboard(), true);
            RegisterReplyKeyboard = new ReplyKeyboardMarkup(GetRegisterReplyKeyboard(), true);
            PasswordKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetPasswordKeyboardInlineKeyboard());
            PortfolioKeyboardInlineKeyboard = new InlineKeyboardMarkup(GetPortfolioKeyboardInlineKeyboard(website.Url));
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
                    new KeyboardButton(Emoji.Link + " " + Localization.Links)
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
                    new InlineKeyboardCallbackButton(Emoji.Keycap7, $"{InlinePrefixKeys.PasswordKey}Num.7"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap8, $"{InlinePrefixKeys.PasswordKey}Num.8"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap9, $"{InlinePrefixKeys.PasswordKey}Num.9")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap4, $"{InlinePrefixKeys.PasswordKey}Num.4"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap5, $"{InlinePrefixKeys.PasswordKey}Num.5"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap6, $"{InlinePrefixKeys.PasswordKey}Num.6")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap1, $"{InlinePrefixKeys.PasswordKey}Num.1"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap2, $"{InlinePrefixKeys.PasswordKey}Num.2"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap3, $"{InlinePrefixKeys.PasswordKey}Num.3")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.LeftArrow, $"{InlinePrefixKeys.PasswordKey}Backspace"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap0, $"{InlinePrefixKeys.PasswordKey}Num.0"),
                    new InlineKeyboardCallbackButton(Emoji.OKButton, $"{InlinePrefixKeys.PasswordKey}Enter")
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
                    new InlineKeyboardCallbackButton(Emoji.NEWButton + " " + Localization.AddJob,
                        $"{InlinePrefixKeys.PortfolioKey}AddJob"),
                    new InlineKeyboardCallbackButton(Emoji.Eye + " " + Localization.ShowJob,
                        $"{InlinePrefixKeys.PortfolioKey}ShowJob")

                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Crayon + " " + Localization.EditJob,
                        $"{InlinePrefixKeys.PortfolioKey}EditJob"),
                    new InlineKeyboardCallbackButton(Emoji.CrossMark + " " + Localization.DeleteJob,
                        $"{InlinePrefixKeys.PortfolioKey}DeleteJob")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.VisitWebsite, url)
                }
            };

            return inlineKeys;
        }

        public InlineKeyboardButton[][] GetLinksKeyboardInlineKeyboard(IWebsiteMiddleware website)
        {
            var inlineKeys = new[]
            {
                string.IsNullOrEmpty(website.Url)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Website}", $"{InlinePrefixKeys.LinksKey}EditWebsite")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditWebsite"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Website, website.Url)
                    },
                string.IsNullOrEmpty(website.TelegramUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Telegram}", $"{InlinePrefixKeys.LinksKey}EditTelegram")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditTelegram"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Telegram, website.TelegramUrl)
                    },
                string.IsNullOrEmpty(website.InstagramUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Instagram}", $"{InlinePrefixKeys.LinksKey}EditInstagram")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditInstagram"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Instagram, website.InstagramUrl)
                    },
                string.IsNullOrEmpty(website.FacebookUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Facebook}", $"{InlinePrefixKeys.LinksKey}EditFacebook")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditFacebook"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Facebook, website.FacebookUrl)
                    },
                string.IsNullOrEmpty(website.GooglePlusUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.GooglePlus}", $"{InlinePrefixKeys.LinksKey}EditGooglePlus")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditGooglePlus"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.GooglePlus, website.GooglePlusUrl)
                    },
                string.IsNullOrEmpty(website.TwitterUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Twitter}", $"{InlinePrefixKeys.LinksKey}EditTwitter")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditTwitter"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Twitter, website.TwitterUrl)
                    },
                string.IsNullOrEmpty(website.LinkedInUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.LinkedIn}", $"{InlinePrefixKeys.LinksKey}EditLinkedIn")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditLinkedIn"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.LinkedIn, website.LinkedInUrl)
                    },
                string.IsNullOrEmpty(website.FlickerUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Flicker}", $"{InlinePrefixKeys.LinksKey}EditFlicker")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{InlinePrefixKeys.LinksKey}EditFlicker"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Flicker, website.FlickerUrl)
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
                    new InlineKeyboardCallbackButton(Emoji.Crayon + " " + Localization.Update,
                        $"{InlinePrefixKeys.AboutKey}Update"),
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
                    new InlineKeyboardCallbackButton(Emoji.JapaneseSymbolForBeginner + " " + Localization.ChangeLogo,
                        $"{InlinePrefixKeys.LogoKey}Update"),
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
                    new InlineKeyboardCallbackButton(Emoji.CrossMarkButton + " " + Localization.Cancel, "Cancel"),
                }
            };

            return inlineKeys;
        }
    }
}