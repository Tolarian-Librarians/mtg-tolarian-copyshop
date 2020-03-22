using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.ObjectModel;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für SelectArtworkChildView.xaml
    /// </summary>
    public partial class SelectArtworkChildView : ChildWindow
    {
        public SelectArtworkChildView(CardController cardController, Guid cardId)
        {
            this.InitializeComponent();

            this.DataContext = new SelectArtworkViewModel(cardController, cardId, new Command(this.HandleAffirmativeCommand));
        }

        private void HandleAffirmativeCommand(object commandParameter)
        {
            if (commandParameter is Guid printId && this.DataContext is SelectArtworkViewModel model)
            {
                // reset collection to interrupt loading of images
                model.Artworks = new ObservableCollection<ArtworkCard>();
                this.Close(printId);
            }
        }
    }
}
