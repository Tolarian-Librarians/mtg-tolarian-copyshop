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
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderViewModel : BindableBase
    {
        #region Fields

        private static DeckBuilderViewModel _deckBuilder;
        private readonly CardController _cardController;
        private readonly DeckController _deckController;
        private readonly DeckCardModel _deckCardModel;

        private int _deckCardCount;
        private FullCardResponse _selectedCard;
        private Visibility _searchResultVisibility = Visibility.Hidden;
        private string _searchText;
        private int _searchResultCount;
        private bool _hasSearchText;
        private ObservableCollection<CardSearchCard> _searchResults;
        private CardSearchCard _selectedSearchItem;
        private int _selectedSearchIndex;
        private Task task;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        #endregion

        #region Constructor

        public DeckBuilderViewModel(CardController controller, DeckController deckController, DeckCardModel deckCardModel)
        {
            _deckBuilder = this;
            this._cardController = controller;
            _deckController = deckController;
            this._deckCardModel = deckCardModel;
            this.IncreaseCardAmountCommand = new Command(this.IncreaseAmountSelectedCard);
            this.ReduceCardAmountCommand = new Command(this.ReduceAmountSelectedCard);
            this.DeleteCardCommand = new Command(this.DeleteSelectedCard);
            this.ApplySelectedSearchItemCommand = new Command(this.ApplySelectedSearchItem);
            this.ClearSearchCommand = new Command(this.ClearSearch);

            this._deckCardModel.DeckCards.CollectionChanged += this.DeckCards_CollectionChanged;
        }

        #endregion

        #region Properties

        public ObservableCollection<FullCard> DeckCards
        {
            get => this._deckCardModel.DeckCards;
            set
            {
                if (!Equals(this._deckCardModel.DeckCards, value))
                {
                    this._deckCardModel.DeckCards = value;
                    this.OnPropertyChanged(nameof(this.DeckCards));
                    DeckViewerViewModel.GetInstance().InvokeDeckCards();
                    this.CalculateDeckCardCount();
                }
            }
        }

        public int DeckCardCount
        {
            get => this._deckCardCount;
            set => this.SetProperty(ref this._deckCardCount, value);
        }

        public FullCard SelectedCard
        {
            get => this._selectedCard;
            set => this.SetProperty(ref this._selectedCard, value);
        }

        public ObservableCollection<CardSearchCard> SearchResults
        {
            get => this._searchResults;
            set => this.SetProperty(ref this._searchResults, value);
        }

        public Visibility SearchResultVisibility
        {
            get => this._searchResultVisibility;
            set => this.SetProperty(ref this._searchResultVisibility, value);
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

        public bool HasSearchText
        {
            get => this._hasSearchText;
            set => this.SetProperty(ref this._hasSearchText, value);
        }


        public CardSearchCard SelectedSearchItem
        {
            get => this._selectedSearchItem;
            set
            {
                this.SetProperty(ref this._selectedSearchItem, value);
                this.OnSelectedSearchItemChanged();
            }
        }

        public int SelectedSearchIndex
        {
            get => this._selectedSearchIndex;
            set => this.SetProperty(ref this._selectedSearchIndex, value);
        }

        public int SearchResultCount
        {
            get => this._searchResultCount;
            set => this.SetProperty(ref this._searchResultCount, value);
        }


        public Command IncreaseCardAmountCommand { get; set; }

        public Command ReduceCardAmountCommand { get; set; }

        public Command DeleteCardCommand { get; set; }

        public Command ApplySelectedSearchItemCommand { get; set; }

        public Command ClearSearchCommand { get; set; }

        #endregion

        #region Static Methods

        public static DeckBuilderViewModel GetInstance()
            => _deckBuilder;

        #endregion

        #region Methods

        public void InvokeDeckCards()
            => this.OnPropertyChanged(nameof(this.DeckCards));

        private void SendErrorMessage(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                CopyShopViewModel.GetInstance().ShowMessage("Error", errorMessage);
            }
        }

        private void DeleteSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCard card)
            {
                this.DeckCards.Remove(card);
            }
        }

        private void DeckCards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => this.CalculateDeckCardCount();

        private void CalculateDeckCardCount()
            => this.DeckCardCount = this._deckController.GetTotalCardCountOfDeck(this.DeckCards.Cast<IFullCard>().ToList());

        private void IncreaseAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCard card)
            {
                this.DeckCards.First(o => o == card).CardCount++;
                this.CalculateDeckCardCount();
            }
        }

        private void ReduceAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCard card)
            {
                if (--this.DeckCards.First(o => o == card).CardCount < 1)
                {
                    this.DeleteSelectedCard(clickedCard);
                }
                this.CalculateDeckCardCount();
            }
        }

        internal void AddCards(IEnumerable<FullCard> cardList, bool asNewList = false)
        {
            if (asNewList)
            {
                CreateNewDeckList();
            }

            if (cardList != null)
            {
                foreach (FullCard card in cardList)
                {
                    this.AddCard(card);
                }
            }
        }

        private void AddCard(FullCard card)
        {
            if (this.DeckCards.FirstOrDefault(o => o.Id == card.Id && o.Name == card.Name) is FullCard ExistingCard)
            {
                ExistingCard.CardCount++;
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => this.DeckCards.Add(card)), DispatcherPriority.Normal);
            }
        }

        private void CreateNewDeckList()
        {
            this._deckCardModel.DeckCards.CollectionChanged -= this.DeckCards_CollectionChanged;
            this.DeckCards = new ObservableCollection<FullCard>();
            this._deckCardModel.DeckCards.CollectionChanged += this.DeckCards_CollectionChanged;
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

        private void OnSearchTextChanged()
        {
            this.HasSearchText = this.SearchText.Length > 0;

            if (this.SearchText.Length < 4)
            {
                this.ResetSearchedItems();
                return;
            }

            var result = this._cardController.GetSearchResults(this.SearchText, 10);
            if (this.token.IsCancellationRequested)
            {
                return;
            }

            this.SendErrorMessage(this._cardController.ErrorMessage);
            this.SearchResultCount = result.ResultsCount;
            this.SearchResults = new ObservableCollection<CardSearchCard>(result.Results);
            this.SearchResultVisibility = Visibility.Visible;
            if (this.SearchResults.Count > 0 && (this.SelectedSearchItem is null || !this.SearchResults.Contains(this.SelectedSearchItem)))
            {
                this.SelectedSearchIndex = 0;
                this.SelectedSearchItem = this.SearchResults[this.SelectedSearchIndex];
            }
        }

        private void OnSelectedSearchItemChanged()
        {

        }

        private void ApplySelectedSearchItem(object _)
        {
            if (this.SelectedSearchItem is null)
            {
                return;
            }

            var newCards = this._cardController.GetCardById(this.SelectedSearchItem.Id);
            this.SendErrorMessage(this._cardController.ErrorMessage);
            this.AddCards(newCards.ConvertAll(card => new FullCard(card)));

            this.SearchText = string.Empty;
            this.ResetSearchedItems();
        }

        private void ResetSearchedItems()
        {
            this.SearchResultCount = 0;
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<CardSearchCard>();
            this.SearchResultVisibility = Visibility.Collapsed;
        }

        private void ClearSearch(object obj)
            => this.SearchText = string.Empty;

        #endregion

    }
}
