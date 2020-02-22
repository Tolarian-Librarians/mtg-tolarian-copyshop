using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfIdentifier
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
