using System.Collections.ObjectModel;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckViewerDesignViewModel
    {
        private readonly DeckBuilderDesignViewModel _deckBuilderDesignViewModel;

        public DeckViewerDesignViewModel()
        {
            this._deckBuilderDesignViewModel = new DeckBuilderDesignViewModel();
            this.DeckCards = this._deckBuilderDesignViewModel.DeckCards;
        }

        public ObservableCollection<FullCardModel> DeckCards { get; set; }
    }
}
