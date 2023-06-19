using System;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfSetInfo
{
    public class SfPaginatedSetList
    {

        [JsonProperty(PropertyName = "data")]

        public SfSet[] Data { get; set; }

        public SfPaginatedSetList GetEmpty()
        {
            return new SfPaginatedSetList
            {
                Data = Array.Empty<SfSet>(),
            };
        }
    }
}