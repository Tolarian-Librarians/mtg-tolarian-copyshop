using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für SelectArtworkChildView.xaml
    /// </summary>
    public partial class SelectArtworkChildView : ChildWindow
    {
        public SelectArtworkChildView(double width, double heigth, CardController cardController, Guid cardId)
        {
            this.InitializeComponent();
            this.Width = width;
            this.Height = heigth;

            this.DataContext = new SelectArtworkViewModel(cardController, cardId, new Command(this.HandleAffirmativeCommand));
        }

        private void HandleAffirmativeCommand(object commandParameter)
        {
            if (commandParameter is Guid printId && this.DataContext is SelectArtworkViewModel model)
            {
                // reset collection to interrupt loading of images
                model.Artworks = new System.Collections.ObjectModel.ObservableCollection<Controller.ResponseObjects.CardArtworkResponse>();
                this.Close(printId);
            }
        }
    }
}
