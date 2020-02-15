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
            this.DeleteCommand = new Command(DeleteSelectedCard);
        }

        #endregion

        #region Properties

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

        public FullCardResponse SelectedCard
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

        public Command DeleteCommand { get; set; }

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
            if (clickedCard is FullCardResponse card)
            {
                this.DeckCards.Remove(card);
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
            }
        }

        private void OnSelectedSearchItemChanged()
        {
            if (this.SelectedSearchItem is null)
            {
                return;
            }

            this.SearchText = string.Empty;
            List<FullCardResponse> newCards = this._controller.GetCardById(this.SelectedSearchItem.Id);
            this.SendErrorMessage(this._controller.ErrorMessage);
            this.DeckCards = new ObservableCollection<FullCardResponse>(this.DeckCards.Concat(newCards));
            this.SelectedSearchItem = null;
            this.SearchResults = new ObservableCollection<CardNameResponse>();
            this.SearchResultVisibility = Visibility.Hidden;
        }

        #endregion

    }
}
