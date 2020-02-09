using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class ImportCardsViewModel : BindableBase
    {
		private string importCard;

		public ImportCardsViewModel(Command affirmativeCommand, Command negativeCommand)
		{
			this.AffirmativeCommand = affirmativeCommand;
			this.NegativeCommand = negativeCommand;
		}

		public string ImportCard
		{
			get => this.importCard;
			set => this.SetProperty(ref this.importCard, value);
		}

        public Command AffirmativeCommand { get; }

		public Command NegativeCommand { get; }

	}
}
