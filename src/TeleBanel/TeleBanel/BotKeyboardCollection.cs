using System;
using System.Collections.Generic;
using System.Linq;
using TeleBanel.Helper;
using TeleBanel.Models;
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
                    new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit} {Localization.ResourceManager.GetString(propName)}", $"{PrefixKeys.AboutKey}Edit{propName}")
                }
            };

            return inlineKeys;
        }

        public IReplyMarkup AboutInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.About)));
        public IReplyMarkup ContactEmailInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.ContactEmail)));
        public IReplyMarkup ContactPhoneInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.ContactPhone)));
        public IReplyMarkup FeedbackEmailInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.FeedbackEmail)));
        public IReplyMarkup TitleInlineKeyboard => new InlineKeyboardMarkup(GetAboutInlineKeyboard(nameof(IWebsiteMiddleware.Title)));
        
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
                    new InlineKeyboardCallbackButton(Emoji.Keycap7, $"{PrefixKeys.PasswordKey}Num.7"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap8, $"{PrefixKeys.PasswordKey}Num.8"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap9, $"{PrefixKeys.PasswordKey}Num.9")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap4, $"{PrefixKeys.PasswordKey}Num.4"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap5, $"{PrefixKeys.PasswordKey}Num.5"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap6, $"{PrefixKeys.PasswordKey}Num.6")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Keycap1, $"{PrefixKeys.PasswordKey}Num.1"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap2, $"{PrefixKeys.PasswordKey}Num.2"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap3, $"{PrefixKeys.PasswordKey}Num.3")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.LeftArrow, $"{PrefixKeys.PasswordKey}Backspace"),
                    new InlineKeyboardCallbackButton(Emoji.Keycap0, $"{PrefixKeys.PasswordKey}Num.0"),
                    new InlineKeyboardCallbackButton(Emoji.OKButton, $"{PrefixKeys.PasswordKey}Enter")
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
                    new InlineKeyboardCallbackButton(Emoji.NEWButton + " " + Localization.AddProduct, $"{PrefixKeys.PortfolioKey}{Localization.AddProduct}"),
                    new InlineKeyboardCallbackButton(Emoji.Eye + " " + Localization.ShowProducts, $"{PrefixKeys.PortfolioKey}{Localization.ShowProducts}")

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
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Website}", $"{PrefixKeys.LinksKey}EditWebsite")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditWebsite"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Website, website.Url)
                    },
                string.IsNullOrEmpty(website.TelegramUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Telegram}", $"{PrefixKeys.LinksKey}EditTelegram")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditTelegram"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Telegram, website.TelegramUrl)
                    },
                string.IsNullOrEmpty(website.InstagramUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Instagram}", $"{PrefixKeys.LinksKey}EditInstagram")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditInstagram"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Instagram, website.InstagramUrl)
                    },
                string.IsNullOrEmpty(website.FacebookUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Facebook}", $"{PrefixKeys.LinksKey}EditFacebook")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditFacebook"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Facebook, website.FacebookUrl)
                    },
                string.IsNullOrEmpty(website.GooglePlusUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.GooglePlus}", $"{PrefixKeys.LinksKey}EditGooglePlus")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditGooglePlus"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.GooglePlus, website.GooglePlusUrl)
                    },
                string.IsNullOrEmpty(website.TwitterUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Twitter}", $"{PrefixKeys.LinksKey}EditTwitter")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditTwitter"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.Twitter, website.TwitterUrl)
                    },
                string.IsNullOrEmpty(website.LinkedInUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.LinkedIn}", $"{PrefixKeys.LinksKey}EditLinkedIn")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditLinkedIn"),
                        new InlineKeyboardUrlButton(Emoji.Link + " " + Localization.LinkedIn, website.LinkedInUrl)
                    },
                string.IsNullOrEmpty(website.FlickerUrl)
                    ? new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.HeavyPlusSign} {Localization.Add} {Localization.Flicker}", $"{PrefixKeys.LinksKey}EditFlicker")
                    }
                    : new InlineKeyboardButton[]
                    {
                        new InlineKeyboardCallbackButton($"{Emoji.Crayon} {Localization.Edit}", $"{PrefixKeys.LinksKey}EditFlicker"),
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
                        $"{PrefixKeys.LogoKey}Update"),
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
                    new InlineKeyboardCallbackButton(Emoji.CrossMarkButton + " " + Localization.Cancel, PrefixKeys.CancelKey)
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
                    new InlineKeyboardCallbackButton(Emoji.Wastebasket, $"{PrefixKeys.InboxKey}Delete_{id}"),
                    new InlineKeyboardUrlButton(replyName, replyLink)
                }
            };

            return new InlineKeyboardMarkup(inlineKeys);
        }
        public IReplyMarkup ProductInlineKeyboard(int productId, int start, int totalCount)
        {
            var count = 1; // count of products in every page
            start = start > totalCount
                ? totalCount - count
                : start < 0 ? 0 : start;

            var pageCount = (int)Math.Ceiling((double)totalCount / count);
            var currentPage = (start / count) + 1;
            var prePage = currentPage - 1 < 1 ? currentPage : currentPage - 1;
            var nextPage = currentPage + 1 > pageCount ? currentPage : currentPage + 1;

            var prefix = PrefixKeys.ProductsTrackBarKey + $"{start}_go";

            var lines = new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton(Emoji.Wastebasket,
                        $"{PrefixKeys.PortfolioKey}{Localization.Delete}_{productId}"),
                    new InlineKeyboardCallbackButton(Emoji.Crayon,
                        $"{PrefixKeys.PortfolioKey}{Localization.Edit}_{productId}")
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardCallbackButton("« 1", prefix + "0"),
                    new InlineKeyboardCallbackButton("‹", prefix + (prePage - 1) * count),
                    new InlineKeyboardCallbackButton($"• {currentPage} •", prefix + start),
                    new InlineKeyboardCallbackButton("›", prefix + (nextPage - 1) * count),
                    new InlineKeyboardCallbackButton($"{pageCount} »", prefix + (pageCount - 1) * count)
                }
            };

            var inlineKeys = new InlineKeyboardMarkup(lines.ToArray());

            return inlineKeys;
        }


    }
}