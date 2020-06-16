using System;
using System.Collections.ObjectModel;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class SelectArtworkViewModel : BindableBase
    {
        #region Fields

        private ObservableCollection<ArtworkCard> _artworks = new ObservableCollection<ArtworkCard>();
        private bool _artworksLoaded;
        private readonly CardController _cardController;
        private readonly Guid _cardId;

        #endregion

        #region ctor

        public SelectArtworkViewModel(CardController cardController, Guid cardId, Command affirmativeCommand)
        {
            this._cardController = cardController;
            this.AffirmativeCommand = affirmativeCommand;
            this._cardId = cardId;
        }

        #endregion

        #region Properties

        public Command AffirmativeCommand { get; }

        public ObservableCollection<ArtworkCard> Artworks
        {
            get => this._artworks;
            set => this.SetProperty(ref this._artworks, value);
        }

        public bool ArtworksLoaded
        {
            get => this._artworksLoaded;
            set => this.SetProperty(ref this._artworksLoaded, value);
        }

        #endregion

        #region Methods

        public void LoadArtworks()
        {
            this.Artworks = new ObservableCollection<ArtworkCard>(this._cardController.GetArtworksOfCard(this._cardId).Artworks);
            this.ArtworksLoaded = true;
        }

        #endregion
    }
}
