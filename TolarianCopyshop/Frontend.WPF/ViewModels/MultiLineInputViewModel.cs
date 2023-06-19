using Tolarian.Copyshop.Fontend.WPF.Base;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class MultiLineInputViewModel : BindableBase
    {
        private string _header;
        private string _multiLineText;

        public MultiLineInputViewModel(Command affirmativeCommand, Command negativeCommand)
        {
            AffirmativeCommand = affirmativeCommand;
            NegativeCommand = negativeCommand;
        }

        public string MultiLineText
        {
            get => _multiLineText;
            set => SetProperty(ref _multiLineText, value);
        }

        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        public Command AffirmativeCommand { get; }

        public Command NegativeCommand { get; }
    }
}