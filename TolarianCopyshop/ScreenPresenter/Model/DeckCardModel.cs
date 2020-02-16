using System.Collections.ObjectModel;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.Model
{
    public class DeckCardModel : BindableBase
    {
        private ObservableCollection<FullCard> _deckCards;

        public DeckCardModel()
        {
            this.DeckCards = new ObservableCollection<FullCard>();
        }

        public ObservableCollection<FullCard> DeckCards
        {
            get => this._deckCards;
            set => this.SetProperty(ref this._deckCards, value);
        }
    }
}
