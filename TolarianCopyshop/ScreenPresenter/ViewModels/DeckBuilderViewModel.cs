using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Models;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderViewModel : BindableBase
    {
        private Card _selectedCard;
        private readonly CardController _controller;
        private ObservableCollection<Card> _cards;

        public DeckBuilderViewModel(CardController controller)
        {
            this._cards = new ObservableCollection<Card>();
            this._controller = controller;
        }

        public ObservableCollection<Card> Cards
        {
            get => this._cards;
            set => this.SetProperty(ref this._cards, value);
        }

        public Card SelectedCard
        {
            get => this._selectedCard;
            set => this.SetProperty(ref this._selectedCard, value);
        }

    }
}
