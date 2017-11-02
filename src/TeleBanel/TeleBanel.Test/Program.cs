using System;
using TeleBanel.Test.MiddlewareModels;

namespace TeleBanel.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TeleBanel (Telegram Bot Panel)");
            Console.WriteLine("Connecting to telegram server...");

            var myWebsite = new WebsiteMiddleware()
            {
                SiteName = "Test.com",
                Url = "https://xomorod.com",
                Title = "Welcome Test Website",
                About = "This is a Test about!",
                ContactEmail = "contact@test.com",
                ContactPhone = "+98-914-914-9202",
                FacebookUrl = "https://facebook.com/test",
                FlickerUrl = null,
                FeedbackEmail = "feedback@test.com",
                GooglePlusUrl = null,
                InstagramUrl = null,
                LinkedInUrl = null,
                TelegramUrl= "https://telegram.me/dezirerobot",
                Logo = Properties.Resources.logo.ToByte()
            };

            var bot =
                new BotManager("414286832:AAE-VQpu32juCfeOWLX33SDnyUZ_xHdfkT0", "8", myWebsite)
                {
                    JobManager = new JobMiddleware(),
                    InboxManager = new InboxMiddleware()
                }; // TestForSelfBot

            bot.StartListeningAsync().Wait();

            Console.Read();
        }
    }
}