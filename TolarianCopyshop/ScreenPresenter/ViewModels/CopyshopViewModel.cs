using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Views;
using static MahApps.Metro.SimpleChildWindow.ChildWindowManager;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class CopyShopViewModel : BindableBase
    {
        private static CopyShopViewModel _copyshop;
        private readonly CardController _controller;
        private readonly IDialogCoordinator _dialogCoordinator;
        private Command importCommand;

        public CopyShopViewModel(CardController controller, DialogCoordinator dialogCoordinator)
        {
            _copyshop = this;
            this._controller = controller;
            this._dialogCoordinator = dialogCoordinator;
        }

        internal static CopyShopViewModel GetInstance()
            => _copyshop;

        public Command ImportCommand
        {
            get
            {
                if (this.importCommand is null)
                {
                    this.importCommand = new Command(this.ImportDeck);
                }
                return this.importCommand;
            }
        }

        private async void ImportDeck(object obj)
        {
            string importCards = await CopyShopView.GetInstance().ShowChildWindowAsync<string>(new ImportCardsChildView() { IsModal = false }).ConfigureAwait(false);

            List<FullCardResponse> importedCards = _controller.GetCardsByNameList(importCards ?? "", out string errMessage);

            if (importedCards.Count > 0)
            {
                DeckBuilderViewModel.GetInstance().DeckCards = new ObservableCollection<FullCardResponse>(importedCards);
            }
            else if (!string.IsNullOrEmpty(errMessage))
            {
                this.ShowMessage("Error", errMessage);
            }
        }

        // Methods
        internal async void ShowMessage(string header, string message)
            => await this._dialogCoordinator.ShowMessageAsync(this, header, message).ConfigureAwait(false);

        internal async void FooProgress()
        {
            // Show...
            ProgressDialogController controller = await this._dialogCoordinator.ShowProgressAsync(this, "HEADER", "MESSAGE").ConfigureAwait(false);
            controller.SetIndeterminate();

            // Do your work...

            // Close...
            await controller.CloseAsync();
        }

    }
}
