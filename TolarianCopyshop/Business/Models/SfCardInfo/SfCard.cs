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
        public Dictionary<string, string> Legalities { get; set; }

        [JsonProperty(PropertyName = "card_faces")]
        public List<SfCardFace> CardFaces { get; set; }

        [JsonProperty(PropertyName = "type_line")]
        public string TypeLine { get; set; }

        [JsonProperty(PropertyName = "cmc")]
        public float ConvertedManaCost { get; set; }

        [JsonProperty(PropertyName = "color_identity")]
        public List<MtgColor> ColorIdentity { get; set; }

        [JsonProperty(PropertyName = "colors")]
        public List<MtgColor> Colors { get; set; }

        [JsonProperty(PropertyName = "produced_mana")]
        public List<MtgColor> ProducedMana { get; set; }

        [JsonProperty(PropertyName = "mana_cost")]
        public string ManaCostLine { get; set; }

        [JsonProperty(PropertyName = "set_name")]
        public string SetName { get; set; }

        [JsonProperty(PropertyName = "set")]
        public string SetCode { get; set; }

        [JsonProperty(PropertyName = "power")]
        public string Power { get; set; }

        [JsonProperty(PropertyName = "toughness")]
        public string Toughness { get; set; }

        [JsonProperty(PropertyName = "released_at")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty(PropertyName = "all_parts")]
        public List<SfRelatedCard> RelatedCards { get; set; }

        [JsonIgnore]
        public bool IsMultifaced { get => CardFaces != null; }

        [JsonIgnore]
        public bool IsTransformable { get => CardFaces != null && ImageUris == null; }

        public override string ToString() => Name;

        public static SfCard GetEmpty() => new SfCard { CardId = Guid.Empty };
    }
}
