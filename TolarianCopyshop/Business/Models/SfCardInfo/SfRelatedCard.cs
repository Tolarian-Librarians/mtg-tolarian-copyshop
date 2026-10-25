using System;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfRelatedCard
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// E.g. "token", "meld_part", "meld_result" or "combo_piece". Kept as string because Scryfall may add new components.
        /// </summary>
        [JsonProperty(PropertyName = "component")]
        public string Type { get; set; }
    }
}