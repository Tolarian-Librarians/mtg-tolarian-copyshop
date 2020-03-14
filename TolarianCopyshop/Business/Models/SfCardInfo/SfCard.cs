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

        [JsonProperty(PropertyName = "oracle_id")]
        public Guid CardId { get; set; }

        [JsonProperty(PropertyName = "id")]
        public Guid PrintId { get; set; }

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

        [JsonProperty(PropertyName = "set_name")]
        public string SetName { get; set; }

        [JsonProperty(PropertyName = "set")]
        public string SetCode { get; set; }

        [JsonIgnore]
        public bool IsMultifaced { get => CardFaces != null; }

        [JsonIgnore]
        public bool IsTransformable { get => CardFaces != null && ImageUris == null; }

        public override string ToString() => Name;

        public static SfCard GetEmpty() => new SfCard { CardId = Guid.Empty };
    }
}
