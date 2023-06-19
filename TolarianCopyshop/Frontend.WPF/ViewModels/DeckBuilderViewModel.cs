using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Fontend.WPF.Base;
using Tolarian.Copyshop.Fontend.WPF.Communication;
using Tolarian.Copyshop.Fontend.WPF.Helper;
using Tolarian.Copyshop.Fontend.WPF.Model;
using Tolarian.Copyshop.Fontend.WPF.Views;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
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
        private bool _isFirstFaceVisible;
        private bool _isSecondFaceVisible;
        private SearchCardType _searchType = SearchCardType.Normal;
        private string _searchTextBoxPlaceHolder = "Search for cards on Scryfall ...";

        private Task _task;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;

        #endregion

        #region Constructor

        public DeckBuilderViewModel(CardController cardController, DeckController deckController, DeckCardModel deckCardModel, Dialogs dialogs)
        {
            _deckBuilder = this;
            _cardController = cardController;
            _deckController = deckController;
            _deckCardModel = deckCardModel;
            _dialogs = dialogs;

            IncreaseCardAmountCommand = new Command(IncreaseAmountSelectedCard);
            ReduceCardAmountCommand = new Command(ReduceAmountSelectedCard);
            DeleteCardCommand = new Command(DeleteSelectedCard);
            ApplySelectedSearchItemCommand = new Command(ApplySelectedSearchItem);
            ClearSearchCommand = new Command(ClearSearch);
            ToggleSearchCommand = new Command(ToggleSearch);
            SelectArtworkCommand = new Command(SelectArtwork);
            TransformCardCommand = new Command(TransformCard);

            _deckCardModel.DeckCards.CollectionChanged += DeckCards_CollectionChanged;
        }

        #endregion

        #region Properties

        public ObservableCollection<FullCardModel> DeckCards
        {
            get => _deckCardModel.DeckCards;
            set
            {
                if (!Equals(_deckCardModel.DeckCards, value))
                {
                    _deckCardModel.DeckCards = value;
                    OnPropertyChanged(nameof(DeckCards));
                    DeckPrintViewModel.GetInstance()?.InvokeDeckCards();
                    CalculateDeckCardCount();
                }
            }
        }

        public int DeckCardCount
        {
            get => _deckCardCount;
            set => SetProperty(ref _deckCardCount, value);
        }

        public FullCardModel SelectedCard
        {
            get => _selectedCard;
            set
            {
                SetProperty(ref _selectedCard, value);
                if (SelectedCard != null)
                {
                    IsFirstFaceVisible = true;
                    IsSecondFaceVisible = false;
                }
            }
        }

        public bool IsFirstFaceVisible
        {
            get => _isFirstFaceVisible;
            set => SetProperty(ref _isFirstFaceVisible, value);
        }

        public bool IsSecondFaceVisible
        {
            get => _isSecondFaceVisible;
            set => SetProperty(ref _isSecondFaceVisible, value);
        }

        public ObservableCollection<SearchCard> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public bool IsSearchResultVisible
        {
            get => _isSearchResultVisible;
            set => SetProperty(ref _isSearchResultVisible, value);
        }

        public bool IsSearchProgressVisible
        {
            get => _isSearchProgressVisible;
            set => SetProperty(ref _isSearchProgressVisible, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                OnSearchTextChangedAsync();
            }
        }

        public bool HasSearchText
        {
            get => _hasSearchText;
            set => SetProperty(ref _hasSearchText, value);
        }

        public SearchCard SelectedSearchItem
        {
            get => _selectedSearchItem;
            set
            {
                if (value != null || string.IsNullOrEmpty(SearchText))
                {
                    SetProperty(ref _selectedSearchItem, value);
                }
                else
                {
                    OnPropertyChanged(nameof(SelectedSearchItem));
                }
            }
        }

        public int SelectedSearchIndex
        {
            get => _selectedSearchIndex;
            set
            {
                if (value >= 0 || (value < 0 && string.IsNullOrEmpty(SearchText)))
                {
                    SetProperty(ref _selectedSearchIndex, value);
                }
                else
                {
                    OnPropertyChanged(nameof(SelectedSearchIndex));
                }
            }
        }

        public string SearchResultCount
        {
            get => _searchResultCount;
            set => SetProperty(ref _searchResultCount, value);
        }

        public enum SearchCardType
        {
            Normal, Token
        }

        public SearchCardType SearchType
        {
            get => _searchType;
            set => SetProperty(ref _searchType, value);
        }

        public string SearchTextBoxPlaceHolder
        {
            get => _searchTextBoxPlaceHolder;
            set => SetProperty(ref _searchTextBoxPlaceHolder, value);
        }

        public string SearchDeckText
        {
            get => _searchDeckText;
            set
            {
                SetProperty(ref _searchDeckText, value);
                ICollectionView deckListView = CollectionViewSource.GetDefaultView(DeckBuilderView.GetInstance()._DeckListView.Items);

                if (string.IsNullOrWhiteSpace(SearchDeckText))
                {
                    deckListView.Filter = null;
                }
                else
                {
                    deckListView.Filter = (obj) =>
                    {
                        if (!string.IsNullOrWhiteSpace(SearchDeckText) && obj is FullCardModel CardModel)
                        {
                            return CardModel.FormattedCardName.Contains(SearchDeckText.Trim(), StringComparison.OrdinalIgnoreCase);
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
            CopyShopView view = CopyShopView.GetInstance();
            Guid result = await _dialogs.ShowChildWindowOnUIThread<Guid>(new SelectArtworkChildView(_cardController, SelectedCard.CardId)
            {
                ChildWindowWidth = view.ActualWidth - 200d,
                ChildWindowHeight = view.ActualHeight - 150d,
            }).ConfigureAwait(false);

            if (result is Guid printId && printId != Guid.Empty && printId != SelectedCard.PrintId)
            {
                int CardCount = SelectedCard.CardCount;
                Application.Current.Dispatcher.Invoke(() => DeleteSelectedCard(SelectedCard));

                FullCardModel newCard = FullCardModel.Create(_cardController.GetCardByPrintId(printId).Card);
                newCard.CardCount = CardCount;

                Application.Current.Dispatcher.Invoke(() => AddCard(newCard));
                SelectedCard = newCard;
            }
        }

        private async void TransformCard(object _)
        {
            Image front = DeckBuilderView.GetInstance()._SelectedImage;
            Image back = DeckBuilderView.GetInstance()._SelectedImageSecondFace;
            Image visibleImage = front.Visibility == Visibility.Visible ? front : back;
            Image invisibleImage = front.Visibility != Visibility.Visible ? front : back;

            DoubleAnimationUsingKeyFrames firstAnimation = (Application.Current.Resources["FirstFlip"] as DoubleAnimationUsingKeyFrames).Clone();
            DoubleAnimationUsingKeyFrames secondAnimation = (Application.Current.Resources["SecondFlip"] as DoubleAnimationUsingKeyFrames).Clone();

            await DoTransformAnimation(visibleImage, firstAnimation).ConfigureAwait(false);

            IsFirstFaceVisible = !IsFirstFaceVisible;
            IsSecondFaceVisible = !IsSecondFaceVisible;

            await DoTransformAnimation(invisibleImage, secondAnimation).ConfigureAwait(false);
            // Revert invisible image - so it's set korrekt if the user changes the SelectedCard
            await DoTransformAnimation(visibleImage, secondAnimation).ConfigureAwait(false);
        }

        private static async Task DoTransformAnimation(Image image, DoubleAnimationUsingKeyFrames animation)
        {
            Storyboard sb = new();
            Storyboard.SetTarget(animation, image);
            sb.Children.Add(animation);
            await sb.BeginAsync().ConfigureAwait(false);
        }

        public void InvokeDeckCards()
            => OnPropertyChanged(nameof(DeckCards));

        private void DeleteSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCardModel card)
            {
                foreach (FullCardModel deleteCard in DeckCards.Where(o => o.CardId == card.CardId).ToList())
                {
                    DeckCards.Remove(deleteCard);
                }
            }
        }

        private void DeckCards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => CalculateDeckCardCount();

        private void CalculateDeckCardCount()
            => DeckCardCount = _deckController?.GetTotalCardCountOfDeck(DeckCards.Cast<IFullCard>().ToList()) ?? 0;

        private void IncreaseAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCardModel card)
            {
                DeckCards.First(o => o.CardId == card.CardId).CardCount++;
                CalculateDeckCardCount();
            }
        }

        private void ReduceAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCardModel card)
            {
                DeckCards.First(o => o.CardId == card.CardId).CardCount--;
                DeleteSelectedCard(DeckCards.FirstOrDefault(o => o.CardCount < 1));
                CalculateDeckCardCount();
            }
        }

        internal void AddCards(IEnumerable<FullCardModel> cardList, bool asNewList = false)
        {
            if (asNewList)
            {
                CreateNewDeckList();
            }

            if (cardList != null)
            {
                foreach (FullCardModel card in cardList)
                {
                    AddCard(card);
                }
            }
        }

        private void AddCard(FullCardModel card)
        {
            if (DeckCards.FirstOrDefault(o => o.CardId == card.CardId) is FullCardModel ExistingCard)
            {
                ExistingCard.CardCount++;
                CalculateDeckCardCount();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(new Action(() => DeckCards.Add(card)), DispatcherPriority.Normal);
            }
        }

        private void CreateNewDeckList()
        {
            _deckCardModel.DeckCards.CollectionChanged -= DeckCards_CollectionChanged;
            DeckCards = new ObservableCollection<FullCardModel>();
            _deckCardModel.DeckCards.CollectionChanged += DeckCards_CollectionChanged;
        }

        private void ToggleSearch(object state)
        {
            if ((bool)state)
            {
                //checked
                SearchType = SearchCardType.Token;
                SearchTextBoxPlaceHolder = "Search for tokens on Scryfall ...";
            }
            else
            {
                //unchecked
                SearchType = SearchCardType.Normal;
                SearchTextBoxPlaceHolder = "Search for cards on Scryfall ...";
            }
        }

        private async void OnSearchTextChangedAsync()
        {
            // Run async to not lock the UI
            if (_task != null && _task.Status != TaskStatus.RanToCompletion)
            {
                _tokenSource.Cancel();
                await _task.ConfigureAwait(false);
            }
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _task = Task.Run(() => OnSearchTextChanged(), _token);
            await _task.ConfigureAwait(false);
        }

        private void OnSearchTextChanged()
        {
            HasSearchText = SearchText.Length > 0;

            if (SearchText.Length < 3)
            {
                ResetSearchedItems();
                return;
            }
            IsSearchResultVisible = true;
            IsSearchProgressVisible = true;

            CardSearchResponse result = SearchType switch
            {
                SearchCardType.Normal => _cardController.GetSearchResults(SearchText, 12),
                SearchCardType.Token => _cardController.GetTokenSearchResults(SearchText),
                _ => new CardSearchResponse(),
            };

            if (_token.IsCancellationRequested)
            {
                return;
            }

            _dialogs.SendErrorMessage(_cardController.GetErrorMessage());
            SearchResultCount = result.ResultsCount;
            SearchResults = new ObservableCollection<SearchCard>(result.Results);
            IsSearchProgressVisible = false;

            if (SearchResults.Count > 0)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    SelectedSearchItem = SearchResults.FirstOrDefault(o => o.PrintId == SelectedSearchItem?.PrintId) ?? SearchResults[0];
                    SelectedSearchIndex = SearchResults.IndexOf(SelectedSearchItem);
                }), DispatcherPriority.Normal);
            }
        }

        private void ApplySelectedSearchItem(object _)
        {
            if (SelectedSearchItem is null)
            {
                return;
            }

            FullCardResponse response = _cardController.GetCardByPrintId(SelectedSearchItem.PrintId);
            _dialogs.SendErrorMessage(_cardController.GetErrorMessage());
            AddCard(FullCardModel.Create(response.Card));

            SearchText = string.Empty;
            ResetSearchedItems();
            DeckBuilderView.GetInstance()._SearchTextBox.Focus();
        }

        private void ResetSearchedItems()
        {
            SearchResultCount = "0";
            SelectedSearchItem = null;
            SearchResults = new ObservableCollection<SearchCard>();
            IsSearchResultVisible = false;
        }

        private void ClearSearch(object obj)
            => SearchText = string.Empty;

        #endregion

    }
}