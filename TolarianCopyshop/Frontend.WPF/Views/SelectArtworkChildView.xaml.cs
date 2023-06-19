using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using MahApps.Metro.SimpleChildWindow;

using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Fontend.WPF.Base;
using Tolarian.Copyshop.Fontend.WPF.ViewModels;

namespace Tolarian.Copyshop.Fontend.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für SelectArtworkChildView.xaml
    /// </summary>
    public partial class SelectArtworkChildView : ChildWindow
    {
        public SelectArtworkChildView(CardController cardController, Guid cardId)
        {
            InitializeComponent();
            Loaded += SelectArtworkChildView_Loaded;
            Closing += SelectArtworkChildView_Closing;
            DataContext = new SelectArtworkViewModel(cardController, cardId, new Command(HandleAffirmativeCommand));
        }

        private void SelectArtworkChildView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            => DataContext = null;

        private void SelectArtworkChildView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SelectArtworkViewModel viewModel)
            {
                Task.Run(() => viewModel.LoadArtworks());
            }
        }

        private void HandleAffirmativeCommand(object commandParameter)
        {
            if (commandParameter is Guid printId && DataContext is SelectArtworkViewModel model)
            {
                // reset collection to interrupt loading of images
                model.Artworks = new ObservableCollection<ArtworkCard>();
                Close(printId);
            }
        }


    }
}