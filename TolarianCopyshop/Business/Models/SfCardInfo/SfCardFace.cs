using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfCardFace
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "oracle_text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "type_line")]
        public string TypeLine { get; set; }

        [JsonProperty(PropertyName = "mana_cost")]
        public string ManaCostLine { get; set; }

        [JsonProperty(PropertyName = "image_uris")]
        public Dictionary<CardImageTypes, Uri> ImageUris { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public List<MtgColor> Colors { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
