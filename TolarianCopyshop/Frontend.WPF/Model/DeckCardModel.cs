using System.Collections.ObjectModel;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.Model
{
    public class DeckCardModel : BindableBase
    {
        private ObservableCollection<FullCardModel> _deckCards;

        public DeckCardModel()
        {
            this.DeckCards = new ObservableCollection<FullCardModel>();
        }

        public ObservableCollection<FullCardModel> DeckCards
        {
            get => this._deckCards;
            set => this.SetProperty(ref this._deckCards, value);
        }
    }
}
