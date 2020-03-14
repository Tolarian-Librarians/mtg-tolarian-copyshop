using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    class SelectArtworkChildViewModel : BindableBase
    {
		public SelectArtworkChildViewModel(Command affirmativeCommand, Command negativeCommand)
		{
			this.AffirmativeCommand = affirmativeCommand;
			this.NegativeCommand = negativeCommand;
		}

		public Command AffirmativeCommand { get; }

		public Command NegativeCommand { get; }
	}
}
