using System.Collections.ObjectModel;

using Tolarian.Copyshop.Fontend.WPF.Base;

namespace Tolarian.Copyshop.Fontend.WPF.Model
{
    public class DeckCardModel : BindableBase
    {
        private ObservableCollection<FullCardModel> _deckCards;

        public DeckCardModel()
        {
            DeckCards = new ObservableCollection<FullCardModel>();
        }

        public ObservableCollection<FullCardModel> DeckCards
        {
            get => _deckCards;
            set => SetProperty(ref _deckCards, value);
        }
    }
}