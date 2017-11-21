using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TeleBanel.Helper;
using TeleBanel.Models;
using TeleBanel.Models.Middlewares;
using TeleBanel.Properties;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TeleBanel
{
    // Bot Manager
    public partial class BotManager
    {
        #region Constructors

        public BotManager(string apiKey, string botPassword, IWebsiteMiddleware websiteInfo)
        {
            Accounts = new Dictionary<int, UserWrapper>();
            BotApiKey = apiKey;
            BotApiPassword = botPassword;
            KeyboardCollection = new BotKeyboardCollection();
            WebsiteManager = websiteInfo;
        }

        #endregion

        public async Task StartListeningAsync()
        {
            Bot = new TelegramBotClient(BotApiKey);
            Bot.StartReceiving();
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += Bot_OnCallbackQuery;

            Me = await Bot.GetMeAsync();
            Console.WriteLine(Localization.Bot_Connected, Me.Username);
        }

        public bool UserAuthenticated(User user, out UserWrapper userWrapper)
        {
            if (!Accounts.ContainsKey(user.Id))
                Accounts[user.Id] = UserWrapper.Factory(user);

            userWrapper = Accounts[user.Id];

            return Accounts[user.Id].IsAuthenticated;
        }


        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            if (UserAuthenticated(e.CallbackQuery.From, out UserWrapper user)) // user authenticated
            {
                try
                {
                    await user.ConcurrencyController.WaitAsync();
                    StartTimeToAnswerCallBack(user);

                    var command = e.CallbackQuery.Data.ToLower();
                    user.LastCallBackQuery = e.CallbackQuery;

                    if (command == Localization.Cancel.ToLower())
                    {
                        user.WaitingMessageQuery = null;
                        await DeleteMessageAsync(e.CallbackQuery.Message);
                    }
                    else if (command.StartsWith(InlinePrefixKeys.PortfolioKey))
                    {
                        await GoNextPortfolioStep(user);
                    }
                    else if (command.StartsWith(InlinePrefixKeys.AboutKey))
                    {
                        await GoNextAboutStep(user);
                    }
                    else if (command.StartsWith(InlinePrefixKeys.LogoKey))
                    {
                        await GoNextLogoStep(user);
                    }
                    else if (command.StartsWith(InlinePrefixKeys.LinksKey))
                    {
                        await GoNextLinksStep(user);
                    }
                    else if (command.StartsWith(InlinePrefixKeys.InboxKey))
                    {
                        await GoNextInboxStep(user);
                    }
                    else
                    {
                        await AnswerCallbackQueryAsync(user);
                    }
                }
                finally
                {
                    user.ConcurrencyController.Release();
                }
            }
            else // Before authenticate
                await GoNextPasswordStep(e);
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var userId = e.Message.From.Id;
            var command = e.Message.Text?.GetNetMessage();

            if (e.Message.Chat.Type != ChatType.Private)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Localization.InvalidRequest);
                return;
            }

            if (command == Localization.GetMyId.ToLower())
            {
                await Bot.SendTextMessageAsync(userId,
                    $"{e.Message.From.FirstName} {e.Message.From.LastName}, ID: {userId}");
            }
            else if (UserAuthenticated(e.Message.From, out UserWrapper user)) // CommonReplyKeyboard
            {
                try
                {
                    await user.ConcurrencyController.WaitAsync();
                    user.LastMessageQuery = e.Message;

                    if (command == Localization.Portfolios.ToLower())
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.Portfolios,
                            replyMarkup: KeyboardCollection.PortfolioInlineKeyboard(WebsiteManager.Url));
                    }
                    else if (command == Localization.About.ToLower())
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.About + ": \n\r" + (WebsiteManager.About ?? "---"),
                            replyMarkup: KeyboardCollection.AboutInlineKeyboard);

                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.Title + ": \n\r" + (WebsiteManager.Title ?? "---"),
                            replyMarkup: KeyboardCollection.TitleInlineKeyboard);

                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.ContactEmail + ": \n\r" + (WebsiteManager.ContactEmail ?? "---"),
                            replyMarkup: KeyboardCollection.ContactEmailInlineKeyboard);

                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.FeedbackEmail + ": \n\r" + (WebsiteManager.FeedbackEmail ?? "---"),
                            replyMarkup: KeyboardCollection.FeedbackEmailInlineKeyboard);

                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            Localization.ContactPhone + ": \n\r" + (WebsiteManager.ContactPhone ?? "---"),
                            replyMarkup: KeyboardCollection.ContactPhoneInlineKeyboard);
                    }
                    else if (command == Localization.Logo.ToLower())
                    {
                        using (var stream = new MemoryStream(WebsiteManager.Logo))
                        {
                            await Bot.SendPhotoAsync(e.Message.Chat.Id,
                                photo: new FileToSend("logo", stream),
                                caption: Localization.Logo,
                                replyMarkup: KeyboardCollection.LogoInlineKeyboard());
                        }
                    }
                    else if (command == Localization.Links.ToLower())
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            $"{Emoji.Link + Emoji.Link}           L  I  N  K  S           {Emoji.Link + Emoji.Link}",
                            replyMarkup: KeyboardCollection.LinksInlineKeyboard(WebsiteManager));
                    }
                    else if (command == Localization.Inbox.ToLower())
                    {
                        await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                            $"{Emoji.SpeechBalloon + Emoji.SpeechBalloon}   Messages   {Emoji.SpeechBalloon + Emoji.SpeechBalloon}");

                        foreach (var msg in InboxManager.GetMessages())
                        {
                            await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                                $"{Emoji.Girl} {msg.Name}:    {msg.Subject}" +
                                $"\n\r{Emoji.SpeechBalloon} {msg.Message}" +
                                $"\n\r<a href='https://mail.google.com/mail/u/0/?view=cm&tf=0&to={msg.Email}&su=feedback+(via+{WebsiteManager.SiteName})&body=%0D%0A--%0D%0Avia+{WebsiteManager.SiteName}&bcc&cc&fs=1'>Reply {msg.Name}</a>",
                                parseMode: ParseMode.Html,
                                replyMarkup: KeyboardCollection.DeleteMessageInlineKeyboard(msg.Id));
                        }
                    }
                    else
                    {
                        if (user.WaitingMessageQuery != null)
                        {
                            var t = typeof(BotManager);
                            var m = t.GetMethod(user.WaitingMessageQuery);
                            m?.Invoke(this, new object[] { user });
                        }
                        else
                            await Bot.SendTextMessageAsync(
                                e.Message.Chat.Id,
                                Localization.PleaseChooseYourOptionDoubleDot,
                                replyMarkup: KeyboardCollection.CommonReplyKeyboard());
                    }
                }
                finally
                {
                    user.ConcurrencyController.Release();
                }
            }
            else // RegisterReplyKeyboard
            {
                if (command == Localization.Start.ToLower())
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard());
                }
                else if (command == Localization.Register.ToLower())
                {
                    Accounts[userId].Password = "";
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id,
                        $"{Emoji.Key} {Localization.Password}: ",
                        replyMarkup: KeyboardCollection.PasswordInlineKeyboard());
                }
                else
                {
                    await Bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.RegisterReplyKeyboard());
                }
            }
        }

    }
}