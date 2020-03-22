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

        private ObservableCollection<ArtworkCard> _artworks;
        private readonly CardController _cardController;

        #endregion

        #region ctor

        public SelectArtworkViewModel(CardController cardController, Guid cardId, Command affirmativeCommand)
		{
            this._cardController = cardController;
            this.AffirmativeCommand = affirmativeCommand;

            Artworks = new ObservableCollection<ArtworkCard>(_cardController.GetArtworksOfCard(cardId).Artworks);
        }

        #endregion

        #region Properties

        public Command AffirmativeCommand { get; }

		public ObservableCollection<ArtworkCard> Artworks
		{
			get => this._artworks;
			set => SetProperty(ref _artworks, value);
		}

        #endregion

        #region Methods

        #endregion
    }
}
