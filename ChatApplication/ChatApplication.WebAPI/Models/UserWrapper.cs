using System.Text.Json.Serialization;

namespace ChatApplication.WebAPI.Models
{
    public class UserWrapper
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}
