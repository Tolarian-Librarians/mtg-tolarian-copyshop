using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tolarian.Copyshop.Business.Models
{
    public class SfIdentifierContainer
    {
        [JsonProperty(PropertyName = "identifiers")]
        public List<SfIdentifier> Identifiers { get; set; }
    }
}
