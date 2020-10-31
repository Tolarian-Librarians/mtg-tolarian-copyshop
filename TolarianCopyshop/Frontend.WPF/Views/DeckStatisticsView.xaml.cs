using System.Windows;
using System.Windows.Controls;
using Tolarian.Copyshop.Fontend.WPF.ViewModels;

namespace Tolarian.Copyshop.Fontend.WPF.Views
{
    /// <summary>
    /// Interaction logic for DeckStatisticsView.xaml
    /// </summary>
    public partial class DeckStatisticsView : UserControl
    {
        public DeckStatisticsView()
        {
            this.InitializeComponent();
        }

        private void DeckStatisticsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is DeckStatisticsViewModel viewModel)
            {
                viewModel.LoadChartData();
            }
        }
    }
}
