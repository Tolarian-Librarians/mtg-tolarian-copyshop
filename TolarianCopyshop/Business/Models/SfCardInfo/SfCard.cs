using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfCard
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "oracle_text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "image_uris")]
        public Dictionary<CardImageTypes, Uri> ImageUris { get; set; }

        [JsonProperty(PropertyName = "legalities")]
        public Dictionary<MtgPlayModes, string> Legalities { get; set; }

        [JsonProperty(PropertyName = "card_faces")]
        public List<SfCardFace> CardFaces { get; set; }

        [JsonProperty(PropertyName = "type_line")]
        public string TypeLine { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }

        public static SfCard GetEmpty()
        {
            return new SfCard { Id = Guid.Empty };
        }
    }
}
