using System.Windows;
using System.Windows.Controls;

using Tolarian.Copyshop.Fontend.WPF.ViewModels;

namespace Tolarian.Copyshop.Fontend.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für DeckViewerView.xaml
    /// </summary>
    public partial class DeckPrintView : UserControl
    {
        private static DeckPrintView _deckPrintView;

        public DeckPrintView()
        {
            InitializeComponent();
            _deckPrintView = this;
            Loaded += DeckViewerView_Loaded;
        }

        public static DeckPrintView GetInstance()
            => _deckPrintView;

        private void DeckViewerView_Loaded(object sender, RoutedEventArgs e)
            => ReloadDocumentPreview();

        internal void ReloadDocumentPreview()
        {
            if (DataContext is DeckPrintViewModel viewModel)
            {
                PrintDocumentPreview.Document = viewModel.GetPrintPages();
            }
        }
    }
}