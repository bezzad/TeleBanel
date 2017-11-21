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
        protected InlineKeyboardButton[][] GetAboutInlineKeyboard(string propName)
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit} {Localization.ResourceManager.GetString(propName)}", $"{InlinePrefixKeys.AboutKey}Edit{propName}")
                }
            };

            return inlineKeys;
        }

        public IReplyMarkup AboutInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.About)));
        public IReplyMarkup ContactEmailInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.ContactEmail)));
        public IReplyMarkup ContactPhoneInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.ContactPhone)));
        public IReplyMarkup FeedbackEmailInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.FeedbackEmail)));
        public IReplyMarkup TitleInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.Title)));

        public IReplyMarkup ProductInlineKeyboard(int productId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Wastebasket, $"{InlinePrefixKeys.PortfolioKey}{Localization.Delete}_{productId}"),
                    new InlineKeyboardCallbackButton(Emoji.Crayon, $"{InlinePrefixKeys.PortfolioKey}{Localization.Edit}_{productId}")
                }
            });
        }
        public IReplyMarkup ProductTrackBarInlineKeyboard(int currentProductIndex, int productsCount)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.LastTrackButton, $"{InlinePrefixKeys.PortfolioKey}{Localization.First}"),
                    new InlineKeyboardCallbackButton(Emoji.ReverseButton, $"{InlinePrefixKeys.PortfolioKey}{Localization.Previous}_{currentProductIndex}"),
                    new InlineKeyboardCallbackButton(currentProductIndex.ToString(), $"{InlinePrefixKeys.PortfolioKey}"),
                    new InlineKeyboardCallbackButton(Emoji.PlayButton, $"{InlinePrefixKeys.PortfolioKey}{Localization.Next}_{currentProductIndex}"),
                    new InlineKeyboardCallbackButton(Emoji.NextTrackButton, $"{InlinePrefixKeys.PortfolioKey}{Localization.Last}")
                }
            });
        }
        public IReplyMarkup CommonReplyKeyboard()
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

            return new ReplyKeyboardMarkup(commonKeyboard, true); ;
        }
        public IReplyMarkup RegisterReplyKeyboard()
        {
            var keyboard = new[]
            {
                new KeyboardButton(Emoji.Key + " " + Localization.Register)
            };

            return new ReplyKeyboardMarkup(keyboard, true);
        }
        public IReplyMarkup PasswordInlineKeyboard()
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

            return new InlineKeyboardMarkup(inlineKeys);
        }
        public IReplyMarkup PortfolioInlineKeyboard(string url)
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.NEWButton + " " + Localization.AddProduct, $"{InlinePrefixKeys.PortfolioKey}{Localization.AddProduct}"),
                    new InlineKeyboardCallbackButton(Emoji.Eye + " " + Localization.ShowProducts, $"{InlinePrefixKeys.PortfolioKey}{Localization.ShowProducts}")

                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.VisitWebsite, url)
                }
            };

            return new InlineKeyboardMarkup(inlineKeys);
        }
        public IReplyMarkup LinksInlineKeyboard(IWebsiteMiddleware website)
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

            return new InlineKeyboardMarkup(inlineKeys);
        }
        public IReplyMarkup LogoInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.JapaneseSymbolForBeginner + " " + Localization.ChangeLogo,
                        $"{InlinePrefixKeys.LogoKey}Update"),
                }
            };

            return new InlineKeyboardMarkup(inlineKeys);
        }
        public IReplyMarkup CancelInlineKeyboard()
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.CrossMarkButton + " " + Localization.Cancel, "Cancel"),
                }
            };

            return new InlineKeyboardMarkup(inlineKeys);
        }
        public IReplyMarkup InboxMessageInlineKeyboard(int id, string replyName, string replyLink)
        {
            var inlineKeys = new[]
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Wastebasket, $"{InlinePrefixKeys.InboxKey}Delete_{id}"),
                    new InlineKeyboardUrlButton(replyName, replyLink)
                }
            };

            return new InlineKeyboardMarkup(inlineKeys);
        }
    }
}