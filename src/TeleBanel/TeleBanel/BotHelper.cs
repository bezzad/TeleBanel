using System.IO;
using System.Threading.Tasks;
using TeleBanel.Models;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace TeleBanel
{
    public static class BotHelper
    {
        public static async Task SendPhotoAsync(this ITelegramBotClient bot, UserWrapper user,  
                                                    string caption, string imageName, byte[] imageBytes)
        {
            var msg = await bot.SendTextMessageAsync(user.LastCallBackQuery.Message.Chat.Id, Localization.PleaseWait);

            using (var stream = new MemoryStream(imageBytes))
            {
                await bot.SendPhotoAsync(user.LastCallBackQuery.Message.Chat.Id, new FileToSend(imageName, stream), caption);
            }

            await bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
        }
    }
}