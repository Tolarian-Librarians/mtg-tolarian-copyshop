using MahApps.Metro.SimpleChildWindow;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für MultiLineInput.xaml
    /// </summary>
    public partial class MultiLineInputChildView : ChildWindow
    {
        public MultiLineInputChildView() : this(string.Empty) { }

        public MultiLineInputChildView(string startText)
        {
            this.InitializeComponent();
            this.DataContext = new MultiLineInputViewModel(new Command(this.HandleAffirmativeCommand), new Command(this.HandleNegativeCommand));
            (this.DataContext as MultiLineInputViewModel).MultiLineText = startText;
        }

        private void HandleAffirmativeCommand(object _)
        {
            if (this.DataContext is MultiLineInputViewModel viewModel)
            {
                this.Close(viewModel.MultiLineText);
            }
        }

        private void HandleNegativeCommand(object _) => this.Close();
    }
}
