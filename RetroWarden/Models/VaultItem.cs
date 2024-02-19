using System.Text.Json;
using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class VaultItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("organizationId")]
        public string OrganizationId { get; set; }
        
        [JsonProperty("folderId")]
        public string FolderId { get; set; }
        
        [JsonProperty("type")]
        public int ItemType { get; set; }
        
        [JsonProperty("name")]
        public string ItemName { get; set; }
        
        [JsonProperty("favorite")]
        public bool IsFavorite { get; set; }
        
        [JsonProperty("notes")]
        public string Notes { get; set; }
        
        [JsonProperty("reprompt")]
        public int Reprompt { get; set; }
        
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("revisionDate")]
        public DateTime RevisionDate { get; set; }

        [JsonProperty("deleteDate")]
        public DateTime DeletionDate { get; set; }
        
        [JsonProperty("login")]
        public Login Login { get; set; }
        
        [JsonProperty("collectionIds")]
        public List<string> CollectionIds { get; set; }
    }
    
}