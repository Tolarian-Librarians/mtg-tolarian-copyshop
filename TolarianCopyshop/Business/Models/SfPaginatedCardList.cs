using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tolarian.Copyshop.Business.Models
{
    public class SfPaginatedCardList
    {
        [JsonProperty(PropertyName = "data")]
        public SfCard[] Data { get; set; }

        [JsonProperty(PropertyName = "has_more")]
        public bool MorePagesAvailable { get; set; }

        [JsonProperty(PropertyName = "total_cards")]
        public int CardCount { get; set; }

        [JsonProperty(PropertyName = "warnings")]
        public string[] Warnings { get; set; }
    }
}
