using System.Windows;
using System.Windows.Controls;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für DeckViewerView.xaml
    /// </summary>
    public partial class DeckPrintView : UserControl
    {
        private static DeckPrintView _deckPrintView;

        public DeckPrintView()
        {
            this.InitializeComponent();
            _deckPrintView = this;
            this.Loaded += this.DeckViewerView_Loaded;
        }

        public static DeckPrintView GetInstance()
            => _deckPrintView;

        private void DeckViewerView_Loaded(object sender, RoutedEventArgs e)
            => this.ReloadDocumentPreview();

        internal void ReloadDocumentPreview()
        {
            if (this.DataContext is DeckPrintViewModel viewModel)
            {
                this.PrintDocumentPreview.Document = viewModel.GetPrintPages(new Size(793.70078740157476, 1122.5196850393702));
            }
        }

    }
}
