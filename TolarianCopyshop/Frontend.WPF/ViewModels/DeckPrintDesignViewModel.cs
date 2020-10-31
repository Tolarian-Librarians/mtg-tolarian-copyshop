using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Fontend.WPF.Communication;
using Tolarian.Copyshop.Fontend.WPF.Model;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class DeckPrintDesignViewModel : DeckPrintViewModel
    {
        public DeckPrintDesignViewModel() : this(null, null, new DeckCardModel(), null) { }

        public DeckPrintDesignViewModel(CardController cardController, PrintController printController, DeckCardModel deckCardModel, Dialogs dialogs)
            : base(cardController, printController, deckCardModel, dialogs)
        {
            this.DeckCards = new DeckBuilderDesignViewModel().DeckCards;
        }
    }
}
