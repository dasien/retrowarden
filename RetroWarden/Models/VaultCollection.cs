using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class VaultCollection
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("organizationId")]
        public string OrganizationId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

    }    
}