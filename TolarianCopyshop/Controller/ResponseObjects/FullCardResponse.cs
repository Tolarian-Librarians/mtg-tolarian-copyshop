using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class FullCardResponse
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public string Text { get; set; }

        public Uri SmallImage { get; set; }

        public Uri PngImage { get; set; }

        public Dictionary<string, string> Legalities1 { get; set; }

        public Dictionary<string, string> Legalities2 { get; set; }
    }
}
