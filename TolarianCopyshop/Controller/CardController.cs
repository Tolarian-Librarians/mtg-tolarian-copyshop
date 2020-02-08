using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Business;

namespace Tolarian.Copyshop.Controller
{
    public class CardController
    {
        private readonly ICardDataPresenter _presenter;
        private readonly ICardDataRequester _requester;

        public CardController(ICardDataPresenter presenter, ICardDataRequester requester)
        {
            _presenter = presenter;
            _requester = requester;
        }
    }
}
