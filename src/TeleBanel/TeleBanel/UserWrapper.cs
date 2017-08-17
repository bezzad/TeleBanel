using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TeleBanel
{
    public class UserWrapper
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageCultures LanguageCulture { get; set; } = LanguageCultures.En;
        public bool IsAuthenticated { get; set; } = false;

        [JsonIgnore]
        internal string Password { get; set; } = "";
        [JsonIgnore]
        public CultureInfo Culture => new CultureInfo(LanguageCulture.ToString());


        public static UserWrapper Factory(Telegram.Bot.Types.User telUser)
        {
            return new UserWrapper
            {
                Id = telUser.Id,
                FirstName = telUser.FirstName,
                LastName = telUser.LastName,
                UserName = telUser.Username
            };
        }
    }
}
