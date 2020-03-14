using Newtonsoft.Json;
using System;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfCardCollection
    {
        [JsonProperty(PropertyName = "not_found")]
        public SfIdentifier[] NotFound { get; set; }

        [JsonProperty(PropertyName = "data")]

        public SfCard[] Data { get; set; }

        public static SfCardCollection GetEmpty()
        {
            return new SfCardCollection
            {
                Data = Array.Empty<SfCard>(),
                NotFound = Array.Empty<SfIdentifier>(),
            };
        }
    }
}
