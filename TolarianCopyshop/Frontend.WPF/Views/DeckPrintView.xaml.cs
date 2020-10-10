using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für DeckViewerView.xaml
    /// </summary>
    public partial class DeckPrintView : UserControl
    {
        public DeckPrintView()
        {
            this.InitializeComponent();
            this.Loaded += this.DeckViewerView_Loaded;
        }

        private void DeckViewerView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is DeckPrintViewModel viewModel)
            {
                FixedDocument page = viewModel.GetPrintPages(new Size(793.70078740157476, 1122.5196850393702));
                this.PrintDocumentPreview.Document = page;
            }
        }
    }
}
