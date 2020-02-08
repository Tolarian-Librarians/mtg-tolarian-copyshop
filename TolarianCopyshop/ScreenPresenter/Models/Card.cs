using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Tolarian.Copyshop.Controller;

namespace Tolarian.Copyshop.ScreenPresenter.Models
{
    public class Card
    {
        public Guid ID { get; set; }

        public BitmapImage Picture { get; set; }

        public BitmapImage PictureLarge { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }
    }
}
