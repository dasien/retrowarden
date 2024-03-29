using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class VaultFolder
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }    
}

