using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfCardFace
    {
        /// <summary>
        /// Only set for reversible cards, where the card itself has no oracle id.
        /// </summary>
        [JsonProperty(PropertyName = "oracle_id")]
        public Guid? CardId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "oracle_text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "type_line")]
        public string TypeLine { get; set; }

        [JsonProperty(PropertyName = "mana_cost")]
        public string ManaCostLine { get; set; }

        [JsonProperty(PropertyName = "image_uris")]
        public SfImageUris ImageUris { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public List<string> Colors { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}