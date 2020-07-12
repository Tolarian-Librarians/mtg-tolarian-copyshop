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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaction logic for DeckStatisticsView.xaml
    /// </summary>
    public partial class DeckStatisticsView : UserControl
    {
        public DeckStatisticsView()
        {
            InitializeComponent();
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
