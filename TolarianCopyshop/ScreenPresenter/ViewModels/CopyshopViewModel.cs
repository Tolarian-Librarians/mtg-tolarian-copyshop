using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class CopyShopViewModel
    {
        private readonly CardController _controller;

        public CopyShopViewModel(CardController controller)
        {
            this._controller = controller;
        }
    }
}
