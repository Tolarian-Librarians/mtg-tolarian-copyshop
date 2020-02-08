using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Models;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    class DeckBuilderViewModel : BindableBase
    {

        public DeckBuilderViewModel()
        {
            this._cards = new ObservableCollection<Card>();
        }

        private ObservableCollection<Card> _cards;

        public ObservableCollection<Card> Cards
        {
            get => this._cards;
            set => this.SetProperty(ref this._cards, value);
        }

        private Card _selectedCard;

        public Card SelectedCard
        {
            get => this._selectedCard;
            set => this.SetProperty(ref this._selectedCard, value);
        }

    }
}
