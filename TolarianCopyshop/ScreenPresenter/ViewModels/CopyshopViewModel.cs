using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Model;
using Tolarian.Copyshop.ScreenPresenter.Views;
using static MahApps.Metro.SimpleChildWindow.ChildWindowManager;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class CopyShopViewModel : BindableBase
    {
        #region Fields

        private static CopyShopViewModel _copyshop;
        private readonly CardController _cardController;
        private readonly DeckController _deckController;
        private readonly PrintController _printController;
        private readonly IDialogCoordinator _dialogCoordinator;

        #endregion

        #region Constructor

        public CopyShopViewModel(CardController cardController, PrintController printController, DeckController deckController, DialogCoordinator dialogCoordinator)
        {
            _copyshop = this;
            this._cardController = cardController;
            this._printController = printController;
            _deckController = deckController;
            this._dialogCoordinator = dialogCoordinator;

            // Commands
            this.NewCommand = new Command(this.NewDeck);
            this.OpenCommand = new Command(this.OpenDeck);
            this.SaveCommand = new Command(this.SaveDeck);
            this.SaveToCommand = new Command(this.SaveDeckTo);
            this.ImportTextCommand = new Command(this.ImportDeck);
            this.ClearCommand = new Command(this.ClearDeck);
            this.PrintCommand = new Command(this.PrintDeck);
        }

        #endregion

        #region Properties

        public Command NewCommand { get; set; }

        public Command OpenCommand { get; set; }

        public Command SaveCommand { get; set; }

        public Command SaveToCommand { get; set; }

        public Command ImportTextCommand { get; }

        public Command PrintCommand { get; }

        public Command ClearCommand { get; set; }

        public string SaveFile { get; set; }

        #endregion

        #region Methods

        internal static CopyShopViewModel GetInstance()
            => _copyshop;

        private void NewDeck(object _)
        {
            if (this.HandleRequestSave())
            {
                this.SaveFile = string.Empty;
                DeckBuilderViewModel.GetInstance().DeckCards = new ObservableCollection<FullCard>();
            }
        }

        private void ClearDeck(object _)
        {
            if (this.HandleRequestSave())
            {
                this.SaveFile = string.Empty;
                DeckBuilderViewModel.GetInstance().DeckCards = new ObservableCollection<FullCard>();
            }
        }

        private void OpenDeck(object _)
        {
            if (this.HandleRequestSave())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    AddExtension = true,
                    DefaultExt = ".xml",
                    Filter = "XML-files (*.xml)|*.xml|All files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),

                };
                if (openFileDialog.ShowDialog() == true)
                {
                    DeckBuilderViewModel.GetInstance().DeckCards = new ObservableCollection<FullCard>(this._deckController.LoadDeckFromFile(openFileDialog.FileName).Cast<FullCard>());
                    this.SaveFile = openFileDialog.FileName;
                }
            }
        }

        private bool HandleRequestSave()
        {
            if (DeckBuilderViewModel.GetInstance().DeckCards.Count == 0)
            {
                return true;
            }
            switch (this.ShowQuestion("Save", "Do you want to save your Deck?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary))
            {
                case MessageDialogResult.Canceled:
                case MessageDialogResult.FirstAuxiliary:
                case MessageDialogResult.SecondAuxiliary:
                    return false;
                case MessageDialogResult.Negative:
                    return true;
                case MessageDialogResult.Affirmative:
                    return this.HandleSave(false);
            }
            return true;
        }

        private bool HandleSave(bool saveAs)
        {
            if (!saveAs && !string.IsNullOrEmpty(this.SaveFile))
            {
                return this._deckController.SaveDeckToFile(this.SaveFile, DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".xml",
                Filter = "XML-files (*.xml)|*.xml|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),

            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.SaveFile = saveFileDialog.FileName;
                return this._deckController.SaveDeckToFile(saveFileDialog.FileName, DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());
            }

            return false;
        }

        private void SaveDeck(object _)
            => this.HandleSave(false);

        private void SaveDeckTo(object _)
            => this.HandleSave(true);

        private async void ImportDeck(object _)
        {
            if (this.HandleRequestSave())
            {
                string importCards = await CopyShopView.GetInstance().ShowChildWindowAsync<string>(new ImportCardsChildView() { IsModal = false }).ConfigureAwait(false);

                this.ShowProgress("IMPORT", "Please wait while your cards getting imported from text...", new Action(() => this.ImportDeckCards(importCards)));
            }
        }

        private void ImportDeckCards(string cards)
        {
            List<IFullCard> importedCards = this._cardController.GetCardsByNameList(cards ?? "");
            this.SendErrorMessage(this._cardController.ErrorMessage);
            if (importedCards.Count > 0)
            {
                DeckBuilderViewModel.GetInstance().DeckCards = new ObservableCollection<FullCard>();
                DeckBuilderViewModel.GetInstance().AddCards(importedCards.ConvertAll(card => new FullCard(card)));
            }
        }

        private void PrintDeck(object _)
        {
            PrintDialog printDlg = new PrintDialog
            {
                PageRangeSelection = PageRangeSelection.AllPages,
                UserPageRangeEnabled = true
            };

            if (printDlg.ShowDialog() == true)
            {
                _printController.PrintDeck(printDlg, DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());
            }
        }

        // Methods
        private void SendErrorMessage(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ShowMessage("Error", errorMessage);
            }
        }

        #endregion

        #region Dialogs

        internal async void ShowMessage(string header, string message)
            => await this._dialogCoordinator.ShowMessageAsync(this, header, message).ConfigureAwait(false);

        internal MessageDialogResult ShowQuestion(string header, string message, MessageDialogStyle style)
            => CopyShopView.GetInstance().ShowModalMessageExternal(header, message, style,
                 new MetroDialogSettings()
                 {
                     AffirmativeButtonText = "YES",
                     NegativeButtonText = "NO",
                     FirstAuxiliaryButtonText = "CANCEL"
                 });

        internal async void ShowProgress(string header, string message, Action FunctionWhileProgress)
        {
            // Show...
            ProgressDialogController controller = await this._dialogCoordinator.ShowProgressAsync(this, header, message).ConfigureAwait(true);
            controller.SetIndeterminate();

            // Do your work...
            await Task.Run(FunctionWhileProgress).ConfigureAwait(true);

            // Close...
            await controller.CloseAsync().ConfigureAwait(true);
        }

        #endregion

    }
}
