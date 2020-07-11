using Newtonsoft.Json;
using System;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfRelatedCard
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "component")]
        public SfRelatedCardType Type { get; set; }
    }
}
