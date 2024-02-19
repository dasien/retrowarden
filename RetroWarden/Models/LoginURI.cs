using System.Text.Json;
using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class LoginURI
    {
        [JsonProperty("uri")]
        public string URI { get; set; }    
        
        [JsonProperty("match")]
        public string Match { get; set; }    
    }
}