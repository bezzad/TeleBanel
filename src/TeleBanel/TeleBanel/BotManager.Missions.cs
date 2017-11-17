using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeleBanel.Helper;
using TeleBanel.Models;
using TeleBanel.Properties;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    public partial class BotManager
    {
        public async Task GoNextPortfolioStep(UserWrapper user)
        {
            var query = user.LastCallBackQuery.Data.Replace(InlinePrefixKeys.PortfolioKey, "");

            if (query == Localization.AddProduct)
            {
            }
            else if (query == Localization.ShowProducts)
            {
                var pids = ProductManager.GetProductsId();
                var product = ProductManager.GetProduct(pids.Length / 2);
                var count = ProductManager.GetProductsId().Length;
                await Bot.SendImageAsync(user, product.Title, product.Descriptin, product.Image, KeyboardCollection.ProductInlineKeyboard(product.Id));

                user.LastMessageQuery = await Bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id,
                    Localization.GotoNextOrPreviousProducts, ParseMode.Markdown,
                    replyMarkup: KeyboardCollection.ProductTrackBarInlineKeyboard(pids.Length / 2, count));
            }
            else if (query == Localization.EditProduct)
            {

            }
            else if (query == Localization.DeleteProduct)
            {

            }
            else if (query.StartsWith(Localization.Previous))
            {
                if (int.TryParse(query.Replace(Localization.Previous, "").Replace("_", ""),
                    out int currentProductIndex) && currentProductIndex > 0)
                {
                    var pids = ProductManager.GetProductsId();
                    var product = ProductManager.GetProduct(pids[currentProductIndex - 1]);
                    await Bot.DeleteMessageAsync(user.LastCallBackQuery.Message.Chat.Id, user.LastCallBackQuery.Message.MessageId);
                    await Bot.SendImageAsync(user, product.Title, product.Descriptin, product.Image, KeyboardCollection.ProductInlineKeyboard(product.Id));
                    user.LastMessageQuery = await Bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id,
                        Localization.GotoNextOrPreviousProducts, ParseMode.Markdown,
                        replyMarkup: KeyboardCollection.ProductTrackBarInlineKeyboard(currentProductIndex - 1, pids.Length));
                }
            }
            else if (query.StartsWith(Localization.Next))
            {
                if (int.TryParse(query.Replace(Localization.Next, "").Replace("_", ""),
                        out int currentProductIndex) && currentProductIndex > 0)
                {
                    var pids = ProductManager.GetProductsId();
                    if (pids.Length > currentProductIndex + 1)
                    {
                        var product = ProductManager.GetProduct(pids[currentProductIndex + 1]);
                        await Bot.DeleteMessageAsync(user.LastCallBackQuery.Message.Chat.Id, user.LastCallBackQuery.Message.MessageId);
                        await Bot.SendImageAsync(user, product.Title, product.Descriptin, product.Image, KeyboardCollection.ProductInlineKeyboard(product.Id));
                        user.LastMessageQuery = await Bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id,
                            Localization.GotoNextOrPreviousProducts, ParseMode.Markdown,
                            replyMarkup: KeyboardCollection.ProductTrackBarInlineKeyboard(currentProductIndex + 1, pids.Length));
                    }
                }
            }

            await AnswerCallbackQueryAsync(user.Id, user.LastCallBackQuery.Id);
        }

        public async Task GoNextAboutStep(UserWrapper user)
        {
            if (user.LastCallBackQuery == null)
                return;

            var propName = user.LastCallBackQuery.Data.Replace(InlinePrefixKeys.AboutKey + "Edit", "");
            if (user.WaitingMessageQuery == null || user.WaitingMessageQuery != nameof(GoNextAboutStep))
            {
                user.WaitingMessageQuery = nameof(GoNextAboutStep);
                await Bot.EditMessageReplyMarkupAsync(user.LastCallBackQuery.Message.Chat.Id,
                    user.LastCallBackQuery.Message.MessageId, KeyboardCollection.CancelInlineKeyboard);

                await AnswerCallbackQueryAsync(user.Id, user.LastCallBackQuery.Id, $"Please enter new {Localization.ResourceManager.GetString(propName)?.ToLower()} and press Enter key.", true);
            }
            else
            {
                var prop = WebsiteManager.GetType().GetProperties().FirstOrDefault(p => p.Name == propName);
                prop?.SetValue(WebsiteManager, user.LastMessageQuery.Text);
                await Bot.EditMessageTextAsync(user.LastCallBackQuery.Message.Chat.Id, user.LastCallBackQuery.Message.MessageId,
                    Localization.ResourceManager.GetString(propName) + ": \n\r" + (prop?.GetValue(WebsiteManager) ?? "---"),
                    ParseMode.Default, false, (IReplyMarkup)KeyboardCollection.GetType().GetProperties().FirstOrDefault(p => p.Name.StartsWith(propName))?.GetValue(KeyboardCollection));

                user.WaitingMessageQuery = null; // waiting method called and then clear buffer
            }
        }

        public async Task GoNextLogoStep(UserWrapper user)
        {
            if (user.LastCallBackQuery == null)
                return;

            if (user.WaitingMessageQuery == null || user.WaitingMessageQuery != nameof(GoNextLogoStep))
            {
                user.WaitingMessageQuery = nameof(GoNextLogoStep);
                await Bot.EditMessageReplyMarkupAsync(user.LastCallBackQuery.Message.Chat.Id,
                    user.LastCallBackQuery.Message.MessageId, KeyboardCollection.CancelInlineKeyboard);

                await AnswerCallbackQueryAsync(user.Id, user.LastCallBackQuery.Id, "Please send an image to change logo ...", true);
            }
            else if (user.LastMessageQuery.Photo.Any())
            {
                using (var mem = new MemoryStream())
                {
                    await Bot.GetFileAsync(user.LastMessageQuery.Photo.Last().FileId, mem);
                    WebsiteManager.Logo = mem.ToByte();
                    await Bot.DeleteMessageAsync(user.LastCallBackQuery.Message.Chat.Id, user.LastCallBackQuery.Message.MessageId);
                    await Bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id, "The logo changed successfully.");
                }
                user.WaitingMessageQuery = null; // waiting method called and then clear buffer
            }
            else
            {
                await AnswerCallbackQueryAsync(user.Id, user.LastCallBackQuery.Id, "Please send an image to change logo ...", true);
            }
        }

        public async Task GoNextLinksStep(UserWrapper user)
        {
            if (user.LastCallBackQuery == null)
                return;

            var linkName = user.LastCallBackQuery.Data.Replace(InlinePrefixKeys.LinksKey + "Edit", "");
            if (user.WaitingMessageQuery == null || user.WaitingMessageQuery != nameof(GoNextLinksStep))
            {
                user.WaitingMessageQuery = nameof(GoNextLinksStep);

                user.LastCallBackQuery.Message = await Bot.EditMessageReplyMarkupAsync(user.LastCallBackQuery.Message.Chat.Id,
                    user.LastCallBackQuery.Message.MessageId, KeyboardCollection.CancelInlineKeyboard);

                await AnswerCallbackQueryAsync(user.Id, user.LastCallBackQuery.Id, $"Please enter new {linkName} link and press Enter key.", true);
            }
            else
            {
                if (Regex.IsMatch(user.LastMessageQuery.Text, StringHelper.UriPattern)
                    && Uri.TryCreate(user.LastMessageQuery.Text, UriKind.RelativeOrAbsolute, out Uri uri)
                    && (uri.Scheme == Uri.UriSchemeHttp
                        || uri.Scheme == Uri.UriSchemeHttps
                        || uri.Scheme == Uri.UriSchemeFtp
                        || uri.Scheme == Uri.UriSchemeMailto))
                {
                    var prop = WebsiteManager.GetType()
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.StartsWith(linkName));

                    prop?.SetValue(WebsiteManager, uri.ToString());

                    await Bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id, "The link updated.");
                    user.LastCallBackQuery.Message = await Bot.EditMessageReplyMarkupAsync(user.LastCallBackQuery.Message.Chat.Id,
                        user.LastCallBackQuery.Message.MessageId, KeyboardCollection.LinksInlineKeyboard);

                    user.WaitingMessageQuery = null; // waiting method called and then clear buffer
                }
                else
                {
                    await Bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id,
                        "Please enter just Uri format like example: http://sampleuri.com");
                }
            }
        }

        public async Task GoNextInboxStep(UserWrapper user)
        {
            if (user.LastCallBackQuery == null)
                return;

            var msgId = user.LastCallBackQuery.Data.Replace(InlinePrefixKeys.InboxKey + "Delete_", "");
            if (user.WaitingMessageQuery == null || user.WaitingMessageQuery != nameof(GoNextInboxStep))
            {
                if (int.TryParse(msgId, out int id))
                {
                    InboxManager.DeleteMessage(id);
                    await Bot.DeleteMessageAsync(user.LastCallBackQuery.Message.Chat.Id,
                        user.LastCallBackQuery.Message.MessageId);
                }
            }
        }

        public async Task<bool> GoNextPasswordStep(CallbackQueryEventArgs e)
        {
            var userId = e.CallbackQuery.From.Id;

            if (!Accounts.ContainsKey(userId))
            {
                await AnswerCallbackQueryAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Id, Localization.EntryPasswordIsIncorrect, showAlert: true);

                return false;
            }

            var data = e.CallbackQuery.Data.ToLower();
            if (!data.StartsWith("password_")) return false;

            if (data.StartsWith("password_num."))
            {
                Accounts[userId].Password += data.Replace("password_num.", "");
            }
            else if (data == "password_enter")
            {
                if (Accounts[userId].Password == BotApiPassword)
                {
                    Accounts[userId].IsAuthenticated = true;

                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    await AnswerCallbackQueryAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Id, Localization.PasswordIsOk, showAlert: true);
                    await Bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        Localization.PleaseChooseYourOptionDoubleDot,
                        replyMarkup: KeyboardCollection.CommonReplyKeyboard);
                    return true;
                }
                else // password is incorrect
                {
                    Accounts[userId].Password = "";
                    await AnswerCallbackQueryAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Id, Localization.EntryPasswordIsIncorrect, showAlert: true);
                    await Bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                    return true;
                }
            }
            else if (data == "password_backspace")
            {
                if (Accounts[userId].Password.Length > 0)
                {
                    Accounts[userId].Password = Accounts[userId]
                        .Password.Remove(Accounts[userId].Password.Length - 1, 1);
                }
            }
            else
            {
                return false;
            }

            await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId,
                $"{Emoji.Key} {Localization.Password}: " + new string(Accounts[userId].Password.Select(x => '*').ToArray()),
                ParseMode.Default, false, KeyboardCollection.PasswordInlineKeyboard);

            return true;
        }
    }
}
