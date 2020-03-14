using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class SelectArtworkViewModel : BindableBase
    {
        #region Fields

        private ObservableCollection<CardArtworkResponse> _artworks;
        private readonly CardController _cardController;

        #endregion

        #region ctor

        public SelectArtworkViewModel(CardController cardController, Guid cardId, Command affirmativeCommand)
		{
            this._cardController = cardController;
            this.AffirmativeCommand = affirmativeCommand;

            Artworks = new ObservableCollection<CardArtworkResponse>(_cardController.GetArtworksOfCard(cardId));
        }

        #endregion

        #region Properties

        public Command AffirmativeCommand { get; }

		public ObservableCollection<CardArtworkResponse> Artworks
		{
			get => this._artworks;
			set => SetProperty(ref _artworks, value);
		}

        #endregion

        #region Methods

        #endregion
    }
}
