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
        #region Fields

        private static DeckBuilderViewModel _deckBuilder;
        private readonly CardController _controller;
        private readonly DeckCardModel _deckCardModel;

        private FullCardResponse _selectedCard;
        private Visibility _searchResultVisibility = Visibility.Hidden;
        private string _searchText;
        private ObservableCollection<CardNameResponse> _searchResults;
        private CardNameResponse _selectedSearchItem;
        private int _selectedSearchIndex;
        private Task task;
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        #endregion

        #region Constructor

        public DeckBuilderViewModel(CardController controller, DeckCardModel deckCardModel)
        {
            _deckBuilder = this;
            this._controller = controller;
            this._deckCardModel = deckCardModel;
            this.IncreaseCardAmountCommand = new Command(this.IncreaseAmountSelectedCard);
            this.ReduceCardAmountCommand = new Command(this.ReduceAmountSelectedCard);
            this.DeleteCardCommand = new Command(this.DeleteSelectedCard);
            this.ApplySelectedSearchItemCommand = new Command(this.ApplySelectedSearchItem);
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
                }
            }
        }

        public FullCard SelectedCard
        {
            get => this._selectedCard;
            set => this.SetProperty(ref this._selectedCard, value);
        }

        public ObservableCollection<CardNameResponse> SearchResults
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

        public CardNameResponse SelectedSearchItem
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


        public Command IncreaseCardAmountCommand { get; set; }

        public Command ReduceCardAmountCommand { get; set; }

        public Command DeleteCardCommand { get; set; }

        public Command ApplySelectedSearchItemCommand { get; set; }

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

        private void IncreaseAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCard card)
            {
                this.DeckCards.First(o => o == card).CardCount++;
            }
        }

        private void ReduceAmountSelectedCard(object clickedCard)
        {
            if (clickedCard is FullCard card && --this.DeckCards.First(o => o == card).CardCount < 1)
            {
                this.DeleteSelectedCard(clickedCard);
            }
        }

        internal void AddCards(IEnumerable<FullCard> cardList)
        {
            foreach (FullCard card in cardList)
            {
                this.AddCard(card);
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
            if (this.SearchText.Length < 4)
            {
                this.ResetSearchedItems();
                return;
            }

            var result = this._controller.GetCardNamesAndIdsBySearchQuery(this.SearchText, 10);
            if (this.token.IsCancellationRequested)
            {
                return;
            }

            this.SendErrorMessage(this._controller.ErrorMessage);
            this.SearchResults = new ObservableCollection<CardNameResponse>(result);
            if (this.SearchResults.Count > 0)
            {
                this.SearchResultVisibility = Visibility.Visible;
                if (this.SelectedSearchItem is null || !this.SearchResults.Contains(this.SelectedSearchItem))
                {
                    this.SelectedSearchIndex = 0;
                    this.SelectedSearchItem = this.SearchResults[this.SelectedSearchIndex];
                }
            }
            else
            {
                this.SearchResultVisibility = Visibility.Collapsed;
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

            var newCards = this._controller.GetCardById(this.SelectedSearchItem.Id);
            this.SendErrorMessage(this._controller.ErrorMessage);
            this.AddCards(newCards.ConvertAll(card => new FullCard(card)));

            this.SearchText = string.Empty;
            this.ResetSearchedItems();
        }

        private void ResetSearchedItems()
        {
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<CardNameResponse>();
            this.SearchResultVisibility = Visibility.Collapsed;
        }

        #endregion

    }
}
