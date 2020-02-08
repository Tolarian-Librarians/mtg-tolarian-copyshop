using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tolarian.Copyshop.ScreenPresenter.Models
{
    public class SearchItem
    {
        public SearchItem(string name)
        {
            this.ID = Guid.Empty;
            this.Name = name;
        }

        public Guid ID { get; set; }

        public string Name { get; set; }
    }
}
