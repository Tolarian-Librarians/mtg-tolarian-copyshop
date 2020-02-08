using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Models;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderViewModel : BindableBase
    {
        private Card _selectedCard;
        private ObservableCollection<Card> _cards;
        private Visibility _seachResultVisibility = Visibility.Hidden;
        private string _searchText;
        private ObservableCollection<SearchItem> _searchResults;
        private SearchItem _selectedSearchItem;
        private readonly CardController _controller;

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

        #region SearchTextBox

        public ObservableCollection<SearchItem> SearchResults
        {
            get => this._searchResults;
            set => this.SetProperty(ref this._searchResults, value);
        }

        public Visibility SeachResultVisibility
        {
            get => this._seachResultVisibility;
            set => this.SetProperty(ref this._seachResultVisibility, value);
        }

        public string SearchText
        {
            get => this._searchText;
            set
            {
                this.SetProperty(ref this._searchText, value);
                this.OnSearchTextChanged();
            }
        }

        public SearchItem SelectedSearchItem
        {
            get => this._selectedSearchItem;
            set
            {
                this.SetProperty(ref this._selectedSearchItem, value);
                this.OnSelectedSearchItemChanged();
            }
        }

        private void OnSearchTextChanged()
        {
            this.SeachResultVisibility = Visibility.Visible;
        }

        private void OnSelectedSearchItemChanged()
        {
            this.SearchText = SelectedSearchItem.Name;
            this.SeachResultVisibility = Visibility.Hidden;
        }

        #endregion SearchTextBox
    }
}
