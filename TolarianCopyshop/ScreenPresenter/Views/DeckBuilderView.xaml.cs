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
    /// Interaktionslogik für DeckBuilder.xaml
    /// </summary>
    public partial class DeckBuilderView : UserControl
    {
        public DeckBuilderView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                _SearchResultBox.Focus();
                _SearchResultBox.SelectedIndex++;
            }
        }
    }
}
