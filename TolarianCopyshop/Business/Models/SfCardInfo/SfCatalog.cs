using System;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfCatalog
    {
        [JsonProperty(PropertyName = "total_values")]
        public int ObjectCount { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string[] Data { get; set; }

        public static SfCatalog GetEmpty()
        {
            return new SfCatalog
            {
                Data = Array.Empty<string>()
            };
        }
    }
}