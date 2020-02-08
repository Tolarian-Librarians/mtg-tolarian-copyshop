using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tolarian.Copyshop.Business.Models
{
    public class SfCard
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
