using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeleBanel.Helper;
using TeleBanel.Models;
using TeleBanel.Properties;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleBanel
{
    // Bot Manager Helpers
    public partial class BotManager
    {
        public static void StartTimeToAnswerCallBack(UserWrapper user)
        {
            user?.LastRequestElapsedTime.Restart();
        }

        public static bool CanAnswerCallBack(UserWrapper user)
        {
            // after 14sec telegram don't permission to send answer to income callback queries
            return user?.LastRequestElapsedTime?.IsRunning == true && user.LastRequestElapsedTime.ElapsedMilliseconds < 13000;
        }

        public static bool CanEditMessage(Message msg)
        {
            if (msg == null) return false;
            return (DateTime.Now - msg.Date).TotalHours < 46; // edit or delete message limit to 48hours
        }

        public async Task AnswerCallbackQueryAsync(UserWrapper user, string text = null, bool showAlert = false, string url = null, int cacheTime = 0)
        {
            if (CanAnswerCallBack(user))
            {
                user.LastRequestElapsedTime?.Stop();
                await Bot.AnswerCallbackQueryAsync(user.LastCallBackQuery?.Id, text, showAlert, url, cacheTime);
            }
        }

        public async Task DeleteMessageAsync(Message msg)
        {
            if (CanEditMessage(msg))
                await Bot.DeleteMessageAsync(msg.Chat.Id, msg.MessageId);
        }

        public async Task SendImageAsync(UserWrapper user,
            string caption, string description, byte[] imageBytes, IReplyMarkup replyMarkup)
        {
            var msg = await Bot.SendTextMessageAsync(user.Id, Localization.PleaseWait);
            await Bot.SendChatActionAsync(user.LastCallBackQuery.Message.Chat.Id, ChatAction.UploadPhoto);

            using (var stream = new MemoryStream(imageBytes))
            {
                user.LastMessageQuery = await Bot.SendPhotoAsync(user.Id, new FileToSend(caption, stream), description, replyMarkup: replyMarkup);
            }

            await DeleteMessageAsync(msg);
        }
    }
}
