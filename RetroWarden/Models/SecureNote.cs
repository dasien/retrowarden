using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class SecureNote : VaultItem
    {
        [JsonProperty("type")]
        public int? Type { get; set; }
    }    
}

