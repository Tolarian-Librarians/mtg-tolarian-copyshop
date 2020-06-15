using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für DeckBuilder.xaml
    /// </summary>
    public partial class DeckBuilderView : UserControl
    {
        public DeckBuilderView()
            => this.InitializeComponent();

        private void SearchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
            => this.HandleSearchUpDown(e);

        private void SearchResultBox_PreviewKeyDown(object sender, KeyEventArgs e)
            => this.HandleSearchUpDown(e);

        private void HandleSearchUpDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (this._SearchResultBox.SelectedIndex != 0)
                    {
                        this.SetSearchResultItem(this._SearchResultBox.SelectedIndex - 1);
                    }
                    else
                    {
                        this._SearchTextBox.Focus();
                    }
                    e.Handled = true;
                    break;
                case Key.Down:
                    this.SetSearchResultItem(this._SearchResultBox.SelectedIndex + 1);
                    e.Handled = true;
                    break;
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.Back:
                    if (!this._SearchTextBox.IsFocused)
                    {
                        this._SearchTextBox.Focus();
                    }
                    break;
            }
        }

        private void SetSearchResultItem(int index)
        {
            if (!this._SearchResultBox.IsFocused)
            {
                this._SearchResultBox.Focus();
            }
            if (this.DataContext is DeckBuilderViewModel oModel && oModel.SearchResults.Count > index)
            {
                oModel.SelectedSearchIndex = index;
                oModel.SelectedSearchItem = oModel.SearchResults.ElementAt(index);
            }
        }

        private void SearchResultBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && this.DataContext is DeckBuilderViewModel oModel)
            {
                oModel.ApplySelectedSearchItemCommand.Execute(new object());
            }
        }
    }
}
