using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Fontend.WPF.Communication;
using Tolarian.Copyshop.Fontend.WPF.Model;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
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
