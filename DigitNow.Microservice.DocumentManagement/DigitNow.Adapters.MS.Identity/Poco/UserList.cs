
using Newtonsoft.Json;

namespace DigitNow.Adapters.MS.Identity.Poco
{
    public class UserList
    {
        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}
