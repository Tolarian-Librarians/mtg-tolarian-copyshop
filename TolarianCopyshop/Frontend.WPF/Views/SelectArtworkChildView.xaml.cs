using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            this.Loaded += this.SelectArtworkChildView_Loaded;
            this.Closing += this.SelectArtworkChildView_Closing;
            this.DataContext = new SelectArtworkViewModel(cardController, cardId, new Command(this.HandleAffirmativeCommand));
        }

        private void SelectArtworkChildView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => this.DataContext = null;

        private void SelectArtworkChildView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext is SelectArtworkViewModel viewModel)
            {
                Task.Run(() => viewModel.LoadArtworks());
            }
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
