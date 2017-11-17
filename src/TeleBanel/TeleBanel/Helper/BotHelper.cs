using System.IO;
using System.Threading.Tasks;
using TeleBanel.Models;
using TeleBanel.Properties;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel.Helper
{
    public static class BotHelper
    {
        public static async Task SendImageAsync(this ITelegramBotClient bot, UserWrapper user,
                                                    string caption, string description, byte[] imageBytes, IReplyMarkup replyMarkup)
        {
            var msg = await bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id, Localization.PleaseWait);

            using (var stream = new MemoryStream(imageBytes))
            {
                user.LastMessageQuery = await bot.SendPhotoAsync(user.LastCallBackQuery.Message.Chat.Id, new FileToSend(caption, stream), description, replyMarkup: replyMarkup);
            }

            await bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
        }

        public static byte[] ToByte(this Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}