using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Communication;
using Tolarian.Copyshop.ScreenPresenter.Model;
using Tolarian.Copyshop.ScreenPresenter.Views;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderViewModel : BindableBase
    {
        #region Fields

        private static DeckBuilderViewModel _deckBuilder;
        private readonly CardController _cardController;
        private readonly DeckController _deckController;
        private readonly DeckCardModel _deckCardModel;
        private readonly Dialogs _dialogs;

        private int _deckCardCount;
        private FullCardModel _selectedCard;
        private bool _isSearchResultVisible;
        private bool _isSearchProgressVisible;
        private string _searchText;
        private string _searchDeckText;
        private string _searchResultCount;
        private bool _hasSearchText;
        private ObservableCollection<SearchCard> _searchResults;
        private SearchCard _selectedSearchItem;
        private int _selectedSearchIndex;
        private SearchCardType _searchType = SearchCardType.Normal;
        private string _searchTextBoxPlaceHolder = "Search for cards on Scryfall ...";

        private Task task;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        #endregion

        #region Constructor

        public DeckBuilderViewModel(CardController cardController, DeckController deckController, DeckCardModel deckCardModel, Dialogs dialogs)
        {
            _deckBuilder = this;
            this._cardController = cardController;
            this._deckController = deckController;
            this._deckCardModel = deckCardModel;
            this._dialogs = dialogs;

            this.IncreaseCardAmountCommand = new Command(this.IncreaseAmountSelectedCard);
            this.ReduceCardAmountCommand = new Command(this.ReduceAmountSelectedCard);
            this.DeleteCardCommand = new Command(this.DeleteSelectedCard);
            this.ApplySelectedSearchItemCommand = new Command(this.ApplySelectedSearchItem);
            this.ClearSearchCommand = new Command(this.ClearSearch);
            this.ToggleSearchCommand = new Command(this.ToggleSearch);
            this.SelectArtworkCommand = new Command(this.SelectArtwork);
            this.TransformCardCommand = new Command(this.TransformCard);

            this._deckCardModel.DeckCards.CollectionChanged += this.DeckCards_CollectionChanged;
        }

        #endregion

        #region Properties

        public ObservableCollection<FullCardModel> DeckCards
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

        public FullCardModel SelectedCard
        {
            get => this._selectedCard;
            set
            {
                this.SetProperty(ref this._selectedCard, value);
                if (this.SelectedCard != null)
                {
                    this.SelectedCard.SelectedCardFace = this.SelectedCard.CardFaces?.First().CroppedImage;
                }
            }
        }

        public ObservableCollection<SearchCard> SearchResults
        {
            get => this._searchResults;
            set => this.SetProperty(ref this._searchResults, value);
        }

        public bool IsSearchResultVisible
        {
            get => this._isSearchResultVisible;
            set => this.SetProperty(ref this._isSearchResultVisible, value);
        }

        public bool IsSearchProgressVisible
        {
            get => this._isSearchProgressVisible;
            set => this.SetProperty(ref this._isSearchProgressVisible, value);
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

        public SearchCard SelectedSearchItem
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
                    this.OnPropertyChanged(nameof(this.SelectedSearchItem));
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
                    this.OnPropertyChanged(nameof(this.SelectedSearchIndex));
                }
            }
        }

        public string SearchResultCount
        {
            get => this._searchResultCount;
            set => this.SetProperty(ref this._searchResultCount, value);
        }

        public enum SearchCardType
        {
            Normal, Token
        }

        public SearchCardType SearchType
        {
            get => this._searchType;
            set => this.SetProperty(ref this._searchType, value);
        }

        public string SearchTextBoxPlaceHolder
        {
            get => this._searchTextBoxPlaceHolder;
            set => this.SetProperty(ref this._searchTextBoxPlaceHolder, value);
        }

        public string SearchDeckText
        {
            get => this._searchDeckText;
            set
            {
                this.SetProperty(ref this._searchDeckText, value);
                ICollectionView deckListView = CollectionViewSource.GetDefaultView(DeckBuilderView.GetInstance()._DeckListView.Items);

                if (string.IsNullOrWhiteSpace(this.SearchDeckText))
                {
                    deckListView.Filter = null;
                }
                else
                {
                    deckListView.Filter = (obj) =>
                    {
                        if (!string.IsNullOrWhiteSpace(this.SearchDeckText) && obj is FullCardModel CardModel)
                        {
                            return CardModel.FormattedCardName.IndexOf(this.SearchDeckText.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        return true;
                    };
                }
            }
        }

        public Command IncreaseCardAmountCommand { get; set; }

        public Command ReduceCardAmountCommand { get; set; }

        public Command DeleteCardCommand { get; set; }

        public Command ApplySelectedSearchItemCommand { get; set; }

        public Command ClearSearchCommand { get; set; }

        public Command SelectArtworkCommand { get; set; }

        public Command TransformCardCommand { get; set; }

        public Command ToggleSearchCommand { get; set; }

        #endregion

        #region Static Methods

        public static DeckBuilderViewModel GetInstance()
            => _deckBuilder;

        #endregion

        #region Methods

        private async void SelectArtwork(object _)
        {
            var view = CopyShopView.GetInstance();
            var result = await this._dialogs.ShowChildWindowOnUIThread<Guid>(new SelectArtworkChildView(this._cardController, this.SelectedCard.CardId)
            {
                ChildWindowWidth = view.ActualWidth - 200d,
                ChildWindowHeight = view.ActualHeight - 150d,
            }).ConfigureAwait(false);

            if (result is Guid printId && printId != Guid.Empty && printId != this.SelectedCard.PrintId)
            {
                int CardCount = this.SelectedCard.CardCount;
                Application.Current.Dispatcher.Invoke(() => this.DeleteSelectedCard(this.SelectedCard));

                FullCardModel newCard = FullCardModel.Create(this._cardController.GetCardByPrintId(printId).Card);
                newCard.CardCount = CardCount;

                Application.Current.Dispatcher.Invoke(() => this.AddCard(newCard));
                this.SelectedCard = newCard;
            }
        }

        private void TransformCard(object _)
        {
            if (this.SelectedCard.SelectedCardFace == this.SelectedCard.CardFaces.First().CroppedImage)
            {
                this.SelectedCard.SelectedCardFace = this.SelectedCard.CardFaces.Last().CroppedImage;
            }
            else
            {
                this.SelectedCard.SelectedCardFace = this.SelectedCard.CardFaces.First().CroppedImage;
            }
        }

        public void InvokeDeckCards()
            => this.OnPropertyChanged(nameof(this.DeckCards));

        private void DeleteSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCardModel card)
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
            if (clickedCard is FullCardModel card)
            {
                this.DeckCards.Where(o => o.CardId == card.CardId).Select(o => o.CardCount++).ToList(); // ToList is needed in order to evaluate the select immediately due to lazy evaluation
                this.CalculateDeckCardCount();
            }
        }

        private void ReduceAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCardModel card)
            {
                this.DeckCards.Where(o => o.CardId == card.CardId).Select(o => --o.CardCount).ToList(); // ToList is needed in order to evaluate the select immediately due to lazy evaluation
                this.DeleteSelectedCard(this.DeckCards.FirstOrDefault(o => o.CardCount < 1));
                this.CalculateDeckCardCount();
            }
        }

        internal void AddCards(IEnumerable<FullCardModel> cardList, bool asNewList = false)
        {
            if (asNewList)
            {
                this.CreateNewDeckList();
            }

            if (cardList != null)
            {
                foreach (FullCardModel card in cardList)
                {
                    this.AddCard(card);
                }
            }
        }

        private void AddCard(FullCardModel card)
        {
            if (this.DeckCards.FirstOrDefault(o => o.CardId == card.CardId) is FullCardModel ExistingCard)
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
            this.DeckCards = new ObservableCollection<FullCardModel>();
            this._deckCardModel.DeckCards.CollectionChanged += this.DeckCards_CollectionChanged;
        }

        private void ToggleSearch(object state)
        {
            if ((bool)state)
            {
                //checked
                this.SearchType = SearchCardType.Token;
                this.SearchTextBoxPlaceHolder = "Search for tokens on Scryfall ...";
            }
            else
            {
                //unchecked
                this.SearchType = SearchCardType.Normal;
                this.SearchTextBoxPlaceHolder = "Search for cards on Scryfall ...";
            }
        }

        private async void OnSearchTextChangedAsync()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChangedAsync: {this._searchText}");
            // Run async to not lock the UI
            if (this.task != null && this.task.Status != TaskStatus.RanToCompletion)
            {
                this.tokenSource.Cancel();
                await this.task.ConfigureAwait(false);
            }
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;
            this.task = Task.Run(() => this.OnSearchTextChanged(), this.token);
            await this.task.ConfigureAwait(false);
        }

        private void OnSearchTextChanged()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): Start");
            this.HasSearchText = this.SearchText.Length > 0;

            if (this.SearchText.Length < 3)
            {
                this.ResetSearchedItems();
                return;
            }
            this.IsSearchResultVisible = true;
            this.IsSearchProgressVisible = true;

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): Request");
            CardSearchResponse result = this.SearchType switch
            {
                SearchCardType.Normal => this._cardController.GetSearchResults(this.SearchText, 12),
                SearchCardType.Token => this._cardController.GetTokenSearchResults(this.SearchText),
                _ => new CardSearchResponse(),
            };
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): Result");

            if (this.token.IsCancellationRequested)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): Task Cancel");
                return;
            }

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): UI Begin");
            this._dialogs.SendErrorMessage(this._cardController.GetErrorMessage());
            this.SearchResultCount = result.ResultsCount;
            this.SearchResults = new ObservableCollection<SearchCard>(result.Results);
            this.IsSearchProgressVisible = false;

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): UI End");
            if (this.SearchResults.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): Invoke Begin");
                    this.SelectedSearchItem = this.SearchResults.FirstOrDefault(o => o.PrintId == this.SelectedSearchItem?.PrintId) ?? this.SearchResults[0];
                    this.SelectedSearchIndex = this.SearchResults.IndexOf(this.SelectedSearchItem);
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): Invoke End");
                }), DispatcherPriority.Normal);
            }
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: OnSearchTextChanged(): End");
        }

        private void ApplySelectedSearchItem(object _)
        {
            if (this.SelectedSearchItem is null)
            {
                return;
            }

            FullCardResponse response = this._cardController.GetCardByPrintId(this.SelectedSearchItem.PrintId);
            this._dialogs.SendErrorMessage(this._cardController.GetErrorMessage());
            this.AddCard(FullCardModel.Create(response.Card));

            this.SearchText = string.Empty;
            this.ResetSearchedItems();
            DeckBuilderView.GetInstance()._SearchTextBox.Focus();
        }

        private void ResetSearchedItems()
        {
            this.SearchResultCount = "0";
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<SearchCard>();
            this.IsSearchResultVisible = false;
        }

        private void ClearSearch(object obj)
            => this.SearchText = string.Empty;

        #endregion

    }
}
