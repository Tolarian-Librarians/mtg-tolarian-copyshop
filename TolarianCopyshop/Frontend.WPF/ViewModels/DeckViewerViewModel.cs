using System.Collections.ObjectModel;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckViewerViewModel : BindableBase
    {
        private static DeckViewerViewModel _deckViewer;
        private readonly CardController _controller;
        private readonly DeckCardModel _deckCardModel;

        public DeckViewerViewModel(CardController controller, DeckCardModel deckCardModel)
        {
            _deckViewer = this;
            this._controller = controller;
            this._deckCardModel = deckCardModel;
        }

        public static DeckViewerViewModel GetInstance()
            => _deckViewer;

        public ObservableCollection<FullCardModel> DeckCards
        {
            get => this._deckCardModel.DeckCards;
            set
            {
                if (!Equals(this._deckCardModel.DeckCards, value))
                {
                    this._deckCardModel.DeckCards = value;
                    this.OnPropertyChanged(nameof(this.DeckCards));
                    DeckBuilderViewModel.GetInstance().InvokeDeckCards();
                }
            }
        }

        public void InvokeDeckCards()
            => this.OnPropertyChanged(nameof(this.DeckCards));
    }
}
