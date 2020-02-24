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
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für MultiLineInput.xaml
    /// </summary>
    public partial class ImportCardsChildView : ChildWindow
    {
        public ImportCardsChildView()
        {
            this.InitializeComponent();
            this.DataContext = new ImportCardsViewModel(new Command(this.HandleAffirmativeCommand), new Command(this.HandleNegativeCommand));
        }

        private void HandleAffirmativeCommand(object _)
        {
            if (this.DataContext is ImportCardsViewModel viewModel)
            {
                this.Close(viewModel.ImportCard);
            }
        }

        private void HandleNegativeCommand(object _)
        {
            this.Close();
        }
    }
}
