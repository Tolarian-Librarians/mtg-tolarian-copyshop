using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Communication;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class CopyShopDesignViewModel : CopyShopViewModel
    {
        public CopyShopDesignViewModel(CardController cardController, PrintController printController, DeckController deckController, ExportController exportController, Dialogs dialogs)
            : base(cardController, printController, deckController, exportController, dialogs) { }
    }
}
