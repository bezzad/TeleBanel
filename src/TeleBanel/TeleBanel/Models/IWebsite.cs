using System;
using System.Collections.Generic;
using System.Text;

namespace TeleBanel.Models
{
    public interface IWebsite
    {
        string SiteName { get; set; }
        string Url { get; set; }
        string About { get; set; }
        string Title { get; set; }
        string ContactEmail { get; set; }
        string FeedbackEmail { get; set; }
        string ContactPhone { get; set; }
        string TelegramUrl { get; set; }
        string InstagramUrl { get; set; }
        string FacebookUrl { get; set; }
        string GooglePlusUrl { get; set; }
        string TwitterUrl { get; set; }
        string LinkedInUrl { get; set; }
        string FlickrUrl { get; set; }
        byte[] Logo { get; set; }
    }
}
