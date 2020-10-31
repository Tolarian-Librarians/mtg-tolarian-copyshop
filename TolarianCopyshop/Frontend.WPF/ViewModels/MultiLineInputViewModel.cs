using Tolarian.Copyshop.Fontend.WPF.Base;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class MultiLineInputViewModel : BindableBase
    {
        private string _header;
        private string _multiLineText;

        public MultiLineInputViewModel(Command affirmativeCommand, Command negativeCommand)
        {
            this.AffirmativeCommand = affirmativeCommand;
            this.NegativeCommand = negativeCommand;
        }

        public string MultiLineText
        {
            get => this._multiLineText;
            set => this.SetProperty(ref this._multiLineText, value);
        }

        public string Header
        {
            get => this._header;
            set => this.SetProperty(ref this._header, value);
        }

        public Command AffirmativeCommand { get; }

        public Command NegativeCommand { get; }
    }
}
