using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderViewModel : BindableBase
    {
        private FullCardResponse _selectedCard;
        private ObservableCollection<FullCardResponse> _cards;
        private Visibility _seachResultVisibility = Visibility.Hidden;
        private string _searchText;
        private ObservableCollection<CardNameResponse> _searchResults;
        private CardNameResponse _selectedSearchItem;
        private readonly CardController _controller;

        public DeckBuilderViewModel(CardController controller)
        {
            this._cards = new ObservableCollection<FullCardResponse>();
            this._controller = controller;
        }

        public ObservableCollection<FullCardResponse> Cards
        {
            get => this._cards;
            set => this.SetProperty(ref this._cards, value);
        }

        public FullCardResponse SelectedCard
        {
            get => this._selectedCard;
            set => this.SetProperty(ref this._selectedCard, value);
        }

        #region SearchTextBox

        public ObservableCollection<CardNameResponse> SearchResults
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

                // Run async to not lock the UI
                Task.Run(() => this.OnSearchTextChanged());
            }
        }

        public CardNameResponse SelectedSearchItem
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
            this.SearchResults = new ObservableCollection<CardNameResponse>(this._controller.GetCardNamesAndIdsBySearchQuery(this.SearchText, 10));
            if (this.SearchResults.Count > 0)
            {
                this.SeachResultVisibility = Visibility.Visible;
            }
        }

        private void OnSelectedSearchItemChanged()
        {
            if (this.SelectedSearchItem is null)
            {
                return;
            }

            this.SearchText = string.Empty;
            this.Cards.Add(this._controller.GetCardById(this.SelectedSearchItem.Id));
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<CardNameResponse>();
            this.SeachResultVisibility = Visibility.Hidden;
        }

        #endregion SearchTextBox
    }
}
