using System;
using System.Collections.ObjectModel;

using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Fontend.WPF.Base;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class SelectArtworkViewModel : BindableBase
    {
        #region Fields

        private ObservableCollection<ArtworkCard> _artworks = new();
        private bool _artworksLoaded;
        private readonly CardController _cardController;
        private readonly Guid _cardId;

        #endregion

        #region ctor

        public SelectArtworkViewModel(CardController cardController, Guid cardId, Command affirmativeCommand)
        {
            _cardController = cardController;
            AffirmativeCommand = affirmativeCommand;
            _cardId = cardId;
        }

        #endregion

        #region Properties

        public Command AffirmativeCommand { get; }

        public ObservableCollection<ArtworkCard> Artworks
        {
            get => _artworks;
            set => SetProperty(ref _artworks, value);
        }

        public bool ArtworksLoaded
        {
            get => _artworksLoaded;
            set => SetProperty(ref _artworksLoaded, value);
        }

        #endregion

        #region Methods

        public void LoadArtworks()
        {
            Artworks = new ObservableCollection<ArtworkCard>(_cardController.GetArtworksOfCard(_cardId).Artworks);
            ArtworksLoaded = true;
        }

        #endregion
    }
}