using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Fontend.WPF.Communication;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class CopyShopDesignViewModel : CopyShopViewModel
    {
        public CopyShopDesignViewModel() : this(null, null, null, null, null) { }

        public CopyShopDesignViewModel(CardController cardController, PrintController printController, DeckController deckController, ExportController exportController, Dialogs dialogs)
            : base(cardController, printController, deckController, exportController, dialogs) { }
    }
}
