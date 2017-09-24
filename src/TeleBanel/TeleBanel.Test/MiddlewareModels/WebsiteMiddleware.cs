using TeleBanel.Models.Middlewares;

namespace TeleBanel.Test.MiddlewareModels
{
    public class WebsiteMiddleware: IWebsiteMiddleware
    {
        public string SiteName { get; set; }
        public string Url { get; set; }
        public string About { get; set; }
        public string Title { get; set; }
        public string ContactEmail { get; set; }
        public string FeedbackEmail { get; set; }
        public string ContactPhone { get; set; }
        public string TelegramUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string GooglePlusUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string FlickrUrl { get; set; }
        public byte[] Logo { get; set; }
    }
}
