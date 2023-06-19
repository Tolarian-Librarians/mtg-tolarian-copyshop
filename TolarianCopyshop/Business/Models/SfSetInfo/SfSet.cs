
using System;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfSetInfo
{
    public class SfSet
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string ScryfallSetCode { get; set; }

        [JsonProperty(PropertyName = "arena_code")]
        public string MagicArenaSetCode { get; set; }

        [JsonProperty(PropertyName = "mtgo_code")]
        public string MagicOnlineSetCode { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string SetName { get; set; }
    }
}