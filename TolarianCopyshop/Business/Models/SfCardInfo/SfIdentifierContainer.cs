using System.Collections.Generic;

using Newtonsoft.Json;

namespace Tolarian.Copyshop.Business.Models.SfCardInfo
{
    public class SfIdentifierContainer
    {
        [JsonProperty(PropertyName = "identifiers")]
        public List<SfIdentifier> Identifiers { get; set; }
    }
}