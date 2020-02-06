using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CardInfoAccess.Model
{
    public class MtgCard
    {
        [JsonProperty("name")]
        public string Name;

        //[JsonProperty("image_uris")]
        //public string[] ImageUris;
    }
}
