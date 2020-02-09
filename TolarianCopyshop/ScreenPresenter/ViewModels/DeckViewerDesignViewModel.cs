using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckViewerDesignViewModel
    {
        private readonly DeckBuilderDesignViewModel _deckBuilderDesignViewModel;

        public DeckViewerDesignViewModel()
        {
            this._deckBuilderDesignViewModel = new DeckBuilderDesignViewModel();
            this.DeckCards = _deckBuilderDesignViewModel.DeckCards;
        }

        public ObservableCollection<FullCardResponse> DeckCards { get; set; }
    }
}
