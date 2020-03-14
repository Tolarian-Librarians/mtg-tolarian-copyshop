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
using Tolarian.Copyshop.ScreenPresenter.Views;
using static MahApps.Metro.SimpleChildWindow.ChildWindowManager;

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
        private string _searchResultCount;
        private bool _hasSearchText;
        private ObservableCollection<CardSearchCard> _searchResults;
        private CardSearchCard _selectedSearchItem;
        private int _selectedSearchIndex;
        private Task task;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        #endregion

        #region Constructor

        public DeckBuilderViewModel(CardController cardController, DeckController deckController, DeckCardModel deckCardModel)
        {
            _deckBuilder = this;
            this._cardController = cardController;
            this._deckController = deckController;
            this._deckCardModel = deckCardModel;
            this.IncreaseCardAmountCommand = new Command(this.IncreaseAmountSelectedCard);
            this.ReduceCardAmountCommand = new Command(this.ReduceAmountSelectedCard);
            this.DeleteCardCommand = new Command(this.DeleteSelectedCard);
            this.ApplySelectedSearchItemCommand = new Command(this.ApplySelectedSearchItem);
            this.ClearSearchCommand = new Command(this.ClearSearch);
            this.SelectArtworkCommand = new Command(this.SelectArtwork);

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
                if (value != null || (value == null && string.IsNullOrEmpty(this.SearchText)))
                {
                    this.SetProperty(ref this._selectedSearchItem, value);
                }
                else
                {
                    this.OnPropertyChanged(nameof(SelectedSearchItem));
                }
            }
        }

        public int SelectedSearchIndex
        {
            get => this._selectedSearchIndex;
            set
            {
                if (value >= 0 || (value < 0 && string.IsNullOrEmpty(this.SearchText)))
                {
                    this.SetProperty(ref this._selectedSearchIndex, value);
                }
                else
                {
                    this.OnPropertyChanged(nameof(SelectedSearchIndex));
                }
            }
        }

        public string SearchResultCount
        {
            get => this._searchResultCount;
            set => this.SetProperty(ref this._searchResultCount, value);
        }

        public Command IncreaseCardAmountCommand { get; set; }

        public Command ReduceCardAmountCommand { get; set; }

        public Command DeleteCardCommand { get; set; }

        public Command ApplySelectedSearchItemCommand { get; set; }

        public Command ClearSearchCommand { get; set; }

        public Command SelectArtworkCommand { get; set; }

        #endregion

        #region Static Methods

        public static DeckBuilderViewModel GetInstance()
            => _deckBuilder;

        #endregion

        #region Methods

        private async void SelectArtwork(object _)
        {
            var view = CopyShopView.GetInstance();
            double childWidth = view.ActualWidth - 200d;
            double childHeight = view.ActualHeight- 150d;

            var result = await CopyShopView.GetInstance().ShowChildWindowAsync<object>(new SelectArtworkChildView(childWidth, childHeight, this._cardController, this.SelectedCard.CardId) { IsModal = false }).ConfigureAwait(false);

            if (result is Guid printId && printId != Guid.Empty)
            {
                int CardCount = this.SelectedCard.CardCount;
                Application.Current.Dispatcher.Invoke(() => this.DeleteSelectedCard(this.SelectedCard));

                var newCards = _cardController.GetCardByPrintId(printId).ConvertAll(card => new FullCard(card));

                foreach (var card in newCards)
                {
                    card.CardCount = CardCount;
                }

                Application.Current.Dispatcher.Invoke(() => this.AddCards(newCards));
            }
        }

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
                foreach (var deleteCard in this.DeckCards.Where(o => o.CardId == card.CardId).ToList())
                {
                    this.DeckCards.Remove(deleteCard);
                }
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
                this.DeckCards.Where(o => o.CardId == card.CardId).Select(o => o.CardCount++).ToList(); // ToList is needed in order to evaluate the select immediately due to lazy evaluation
                this.CalculateDeckCardCount();
            }
        }

        private void ReduceAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCard card)
            {
                this.DeckCards.Where(o => o.CardId == card.CardId).Select(o => --o.CardCount).ToList(); // ToList is needed in order to evaluate the select immediately due to lazy evaluation
                this.DeleteSelectedCard(this.DeckCards.FirstOrDefault(o => o.CardCount < 1));
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
            if (this.DeckCards.FirstOrDefault(o => o.CardId == card.CardId && o.Name == card.Name) is FullCard ExistingCard)
            {
                ExistingCard.CardCount++;
                this.CalculateDeckCardCount();
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

            if (this.SearchText.Length < 3)
            {
                this.ResetSearchedItems();
                return;
            }

            var result = this._cardController.GetSearchResults(this.SearchText, 12);
            if (this.token.IsCancellationRequested)
            {
                return;
            }

            this.SendErrorMessage(this._cardController.ErrorMessage);
            this.SearchResultCount = result.ResultsCount;
            this.SearchResults = new ObservableCollection<CardSearchCard>(result.Results);
            this.SearchResultVisibility = Visibility.Visible;
            if (this.SearchResults.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this.SelectedSearchItem = this.SearchResults.FirstOrDefault(o => o.PrintId == this.SelectedSearchItem?.PrintId) ?? this.SearchResults[0];
                    this.SelectedSearchIndex = this.SearchResults.IndexOf(this.SelectedSearchItem);
                }), DispatcherPriority.Normal);
            }
        }

        private void ApplySelectedSearchItem(object _)
        {
            if (this.SelectedSearchItem is null)
            {
                return;
            }

            var newCards = this._cardController.GetCardByPrintId(this.SelectedSearchItem.PrintId);
            this.SendErrorMessage(this._cardController.ErrorMessage);
            this.AddCards(newCards.ConvertAll(card => new FullCard(card)));

            this.SearchText = string.Empty;
            this.ResetSearchedItems();
        }

        private void ResetSearchedItems()
        {
            this.SearchResultCount = "0";
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<CardSearchCard>();
            this.SearchResultVisibility = Visibility.Collapsed;
        }

        private void ClearSearch(object obj)
            => this.SearchText = string.Empty;

        #endregion

    }
}
