using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class Organization
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("type")]
        public int OrgType { get; set; }
        
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }


    }    
}