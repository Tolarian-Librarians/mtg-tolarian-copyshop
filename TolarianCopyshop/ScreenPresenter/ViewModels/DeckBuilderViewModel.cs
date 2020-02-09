using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderViewModel : BindableBase
    {
        private static DeckBuilderViewModel _deckBuilder;
        private readonly CardController _controller;
        private readonly DeckCardModel _deckCardModel;

        private FullCardResponse _selectedCard;
        private Visibility _seachResultVisibility = Visibility.Hidden;
        private string _searchText;
        private ObservableCollection<CardNameResponse> _searchResults;
        private CardNameResponse _selectedSearchItem;

        public DeckBuilderViewModel(CardController controller, DeckCardModel deckCardModel)
        {
            _deckBuilder = this;
            this._controller = controller;
            this._deckCardModel = deckCardModel;
        }

        public ObservableCollection<FullCardResponse> DeckCards
        {
            get => this._deckCardModel.DeckCards;
            set
            {
                if (!Equals(this._deckCardModel.DeckCards, value))
                {
                    this._deckCardModel.DeckCards = value;
                    this.OnPropertyChanged(nameof(this.DeckCards));
                    DeckViewerViewModel.GetInstance().InvokeDeckCards();
                }
            }
        }

        public void InvokeDeckCards()
            => this.OnPropertyChanged(nameof(this.DeckCards));

        public FullCardResponse SelectedCard
        {
            get => this._selectedCard;
            set => this.SetProperty(ref this._selectedCard, value);
        }

        public static DeckBuilderViewModel GetInstance()
            => _deckBuilder;

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
                this.OnSearchTextChangedAsync();
            }
        }

        private async void OnSearchTextChangedAsync()
        {
            Console.WriteLine($"OnSearchTextChangedAsync: {this._searchText} at {DateTime.Now}");
            // Run async to not lock the UI
            if (task != null && this.task.Status != TaskStatus.RanToCompletion)
            {
                tokenSource.Cancel();
                await this.task.ConfigureAwait(false);
            }
            this.tokenSource = new CancellationTokenSource();
            this.token = tokenSource.Token;
            this.task = Task.Run(() => OnSearchTextChanged(), this.token);
            await this.task.ConfigureAwait(false);
        }

        Task task;
        CancellationTokenSource tokenSource;
        CancellationToken token;


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
            var result = this._controller.GetCardNamesAndIdsBySearchQuery(this.SearchText, 10, out string errMessage);
            if (this.token.IsCancellationRequested)
            {
                return;
            }

            this.SearchResults = new ObservableCollection<CardNameResponse>(result);
            if (this.SearchResults.Count > 0)
            {
                this.SeachResultVisibility = Visibility.Visible;
            }
            else if (!string.IsNullOrEmpty(errMessage))
            {
                CopyShopViewModel.GetInstance().ShowMessage("Error", errMessage);
            }
        }

        private void OnSelectedSearchItemChanged()
        {
            if (this.SelectedSearchItem is null)
            {
                return;
            }

            this.SearchText = string.Empty;
            var newCards = this._controller.GetCardById(this.SelectedSearchItem.Id, out string errMessage);
            this.DeckCards = new ObservableCollection<FullCardResponse>(this.DeckCards.Concat(newCards));
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<CardNameResponse>();
            this.SeachResultVisibility = Visibility.Hidden;
        }

        #endregion SearchTextBox
    }
}
