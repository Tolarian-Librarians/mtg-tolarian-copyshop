using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.Models
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

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
