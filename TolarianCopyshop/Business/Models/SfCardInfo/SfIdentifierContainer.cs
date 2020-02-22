using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfIdentifierContainer
    {
        [JsonProperty(PropertyName = "identifiers")]
        public List<SfIdentifier> Identifiers { get; set; }
    }
}
