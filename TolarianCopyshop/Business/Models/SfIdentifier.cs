using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models
{
    public class SfIdentifier
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
