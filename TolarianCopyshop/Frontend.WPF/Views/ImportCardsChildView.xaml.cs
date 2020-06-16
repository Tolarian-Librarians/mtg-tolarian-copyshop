using MahApps.Metro.SimpleChildWindow;
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
