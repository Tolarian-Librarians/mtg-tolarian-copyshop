using MahApps.Metro.SimpleChildWindow;

using Tolarian.Copyshop.Fontend.WPF.Base;
using Tolarian.Copyshop.Fontend.WPF.ViewModels;

namespace Tolarian.Copyshop.Fontend.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für MultiLineInput.xaml
    /// </summary>
    public partial class MultiLineInputChildView : ChildWindow
    {
        public MultiLineInputChildView() : this("MultiLineInput", string.Empty) { }

        public MultiLineInputChildView(string header, string startText)
        {
            InitializeComponent();
            DataContext = new MultiLineInputViewModel(new Command(HandleAffirmativeCommand), new Command(HandleNegativeCommand));

            if (DataContext is MultiLineInputViewModel viewModel)
            {
                viewModel.Header = header;
                viewModel.MultiLineText = startText;
            }

        }

        private void HandleAffirmativeCommand(object _)
        {
            if (DataContext is MultiLineInputViewModel viewModel)
            {
                Close(viewModel.MultiLineText);
            }
        }

        private void HandleNegativeCommand(object _) => Close();
    }
}