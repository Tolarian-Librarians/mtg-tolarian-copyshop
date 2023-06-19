using System;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
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

        [JsonProperty(PropertyName = "next_page")]
        public Uri NextPageUri { get; set; }

        public static SfPaginatedCardList GetEmpty()
        {
            return new SfPaginatedCardList
            {
                Data = Array.Empty<SfCard>()
            };
        }
    }
}