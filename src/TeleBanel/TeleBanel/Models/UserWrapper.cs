using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Telegram.Bot.Types;

namespace TeleBanel.Models
{
    public class UserWrapper: IUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public string Language { get; set; } = "En";
        public bool IsAuthenticated { get; set; } = false;
        public Message LastMessageQuery { get; set; } = null;
        public CallbackQuery LastCallBackQuery { get; set; } = null;

        [JsonIgnore]
        internal string Password { get; set; } = "";
        [JsonIgnore]
        public CultureInfo Culture => new CultureInfo(Language);

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
