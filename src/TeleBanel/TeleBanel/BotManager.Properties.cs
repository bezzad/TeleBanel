using System.Collections.Generic;
using TeleBanel.Models;
using TeleBanel.Models.Middlewares;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeleBanel
{
    // Bot Manager Properties
    public partial class BotManager
    {
        protected string BotApiPassword { get; set; }
        public string BotApiKey { get; set; }
        public Dictionary<int, UserWrapper> Accounts { get; set; }
        public TelegramBotClient Bot { get; set; }
        public BotKeyboardCollection KeyboardCollection { get; set; }
        public User Me { get; set; }

        public IWebsiteMiddleware WebsiteManager { set; get; }
        public IProductMiddleware ProductManager { set; get; }
        public IInboxMiddleware InboxManager { set; get; }


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
    }
}