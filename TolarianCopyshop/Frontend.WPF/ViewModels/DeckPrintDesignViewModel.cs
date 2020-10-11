using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Communication;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckPrintDesignViewModel : DeckPrintViewModel
    {
        private readonly DeckBuilderDesignViewModel _deckBuilderDesignViewModel;

        public DeckPrintDesignViewModel(CardController cardController, PrintController printController, DeckCardModel deckCardModel, Dialogs dialogs)
            : base(cardController, printController, deckCardModel, dialogs)
        {
            this._deckBuilderDesignViewModel = new DeckBuilderDesignViewModel(cardController, null, deckCardModel, dialogs);
            this.DeckCards = this._deckBuilderDesignViewModel.DeckCards;
        }
    }
}
