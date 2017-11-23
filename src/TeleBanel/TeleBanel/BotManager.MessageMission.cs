using System.IO;
using System.Threading.Tasks;
using TeleBanel.Helper;
using TeleBanel.Models;
using TeleBanel.Properties;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBanel
{
    // BotManager.MessageMission
    public partial class BotManager
    {
        protected async Task OnMessageRegister(int userId)
        {
            Accounts[userId].Password = "";
            await Bot.SendTextMessageAsync(userId,
                $"{Emoji.Key} {Localization.Password}: ",
                replyMarkup: KeyboardCollection.PasswordInlineKeyboard());
        }
        protected async Task OnMessageBeforeRegister(int userId)
        {
            await Bot.SendTextMessageAsync(userId,
                Localization.PleaseRegisterBeforeEnterAnyCommands,
                replyMarkup: KeyboardCollection.RegisterReplyKeyboard());
        }

        protected async Task OnMessageStart(UserWrapper user)
        {
            await Bot.SendTextMessageAsync(
                user.LastMessageQuery.Chat.Id,
                Localization.PleaseChooseYourOptionDoubleDot,
                replyMarkup: KeyboardCollection.CommonReplyKeyboard());
        }

        protected async Task OnMessagePortfolios(UserWrapper user)
        {
            await Bot.SendChatActionAsync(user.LastMessageQuery.Chat.Id, ChatAction.Typing);
            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                Localization.Portfolios,
                replyMarkup: KeyboardCollection.PortfolioInlineKeyboard(WebsiteManager.Url));
        }

        protected async Task OnMessageAbout(UserWrapper user)
        {
            await Bot.SendChatActionAsync(user.LastMessageQuery.Chat.Id, ChatAction.Typing);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                Localization.About + ": \n\r" + (WebsiteManager.About ?? "---"),
                replyMarkup: KeyboardCollection.AboutInlineKeyboard);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                Localization.Title + ": \n\r" + (WebsiteManager.Title ?? "---"),
                replyMarkup: KeyboardCollection.TitleInlineKeyboard);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                Localization.ContactEmail + ": \n\r" + (WebsiteManager.ContactEmail ?? "---"),
                replyMarkup: KeyboardCollection.ContactEmailInlineKeyboard);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                Localization.FeedbackEmail + ": \n\r" + (WebsiteManager.FeedbackEmail ?? "---"),
                replyMarkup: KeyboardCollection.FeedbackEmailInlineKeyboard);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                Localization.ContactPhone + ": \n\r" + (WebsiteManager.ContactPhone ?? "---"),
                replyMarkup: KeyboardCollection.ContactPhoneInlineKeyboard);
        }

        protected async Task OnMessageLogo(UserWrapper user)
        {
            await Bot.SendChatActionAsync(user.LastMessageQuery.Chat.Id, ChatAction.UploadPhoto);

            using (var stream = new MemoryStream(WebsiteManager.Logo))
            {
                await Bot.SendPhotoAsync(user.LastMessageQuery.Chat.Id,
                    photo: new FileToSend("logo", stream),
                    caption: Localization.Logo,
                    replyMarkup: KeyboardCollection.LogoInlineKeyboard());
            }
        }

        protected async Task OnMessageLinks(UserWrapper user)
        {
            await Bot.SendChatActionAsync(user.LastMessageQuery.Chat.Id, ChatAction.Typing);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                $"{Emoji.Link + Emoji.Link}           L  I  N  K  S           {Emoji.Link + Emoji.Link}",
                replyMarkup: KeyboardCollection.LinksInlineKeyboard(WebsiteManager));
        }

        protected async Task OnMessageInbox(UserWrapper user)
        {
            await Bot.SendChatActionAsync(user.LastMessageQuery.Chat.Id, ChatAction.Typing);

            await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                $"{Emoji.SpeechBalloon + Emoji.SpeechBalloon}   Messages   {Emoji.SpeechBalloon + Emoji.SpeechBalloon}");

            foreach (var msg in InboxManager.GetMessages())
            {
                var replyLink =
                    $"https://mail.google.com/mail/u/0/?view=cm&tf=0&to={msg.Email}&su=feedback+(via+{WebsiteManager.SiteName})&body=%0D%0A--%0D%0Avia+{WebsiteManager.SiteName}&bcc&cc&fs=1";

                await Bot.SendTextMessageAsync(user.LastMessageQuery.Chat.Id,
                    $"{Emoji.WomanArtistLightSkinTone} {msg.Name}:    {msg.Subject}\n\r\n\r{Emoji.SpeechBalloon} {msg.Message}", parseMode: ParseMode.Html,
                    replyMarkup: KeyboardCollection.InboxMessageInlineKeyboard(msg.Id, $"Reply {msg.Name}", replyLink));
            }
        }
        
    }
}