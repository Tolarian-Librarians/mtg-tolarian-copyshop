using Newtonsoft.Json;
using System;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfIdentifier
    {
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "set", NullValueHandling = NullValueHandling.Ignore)]
        public string SetCode { get; set; }

        [JsonProperty(PropertyName = "id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }
    }
}
