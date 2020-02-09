using System.Collections.ObjectModel;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.Model
{
    public class DeckCardModel : BindableBase
    {
        private ObservableCollection<FullCardResponse> _deckCards;

        public DeckCardModel()
        {
            this.DeckCards = new ObservableCollection<FullCardResponse>();
        }

        public ObservableCollection<FullCardResponse> DeckCards
        {
            get => this._deckCards;
            set => this.SetProperty(ref this._deckCards, value);
        }
    }
}
