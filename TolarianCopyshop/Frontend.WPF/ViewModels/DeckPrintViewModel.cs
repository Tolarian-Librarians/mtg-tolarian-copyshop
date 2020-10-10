using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckPrintViewModel : BindableBase
    {
        private static DeckPrintViewModel _deckViewer;
        private readonly CardController _cardController;
        private readonly DeckCardModel _deckCardModel;
        private readonly PrintController _printController;

        public DeckPrintViewModel(CardController cardController, PrintController printController, DeckCardModel deckCardModel)
        {
            _deckViewer = this;
            this._cardController = cardController;
            this._deckCardModel = deckCardModel;
            this._printController = printController;
        }

        public static DeckPrintViewModel GetInstance()
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

        public FixedDocument GetPrintPages(System.Windows.Size pageSize)
            => this._printController.GetPrintPages(pageSize, this.DeckCards.Cast<IFullCard>().ToList());
    }
}
