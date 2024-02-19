using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class Login
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("totp")]
        public string TOTP { get; set; }

        [JsonProperty("passwordRevisionDate")]
        public DateTime PasswordRevisionDate { get; set; }

        [JsonProperty("urls")]
        public List<LoginURI> URLs { get; set; }
    }    
}

