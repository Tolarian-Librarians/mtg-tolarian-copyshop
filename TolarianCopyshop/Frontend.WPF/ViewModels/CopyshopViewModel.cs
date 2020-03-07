using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            this.ImportCommand = new Command(this.ImportDeck);
            this.ClearCommand = new Command(this.ClearDeck);
            this.PrintCommand = new Command(this.PrintDeck);
            this.OpenLinkCommand = new Command(this.OpenLink);
        }

        #endregion

        #region Properties

        public Command NewCommand { get; }

        public Command OpenCommand { get; }

        public Command SaveCommand { get; }

        public Command ImportCommand { get; }

        public Command PrintCommand { get; }

        public Command ClearCommand { get; }

        public Command OpenLinkCommand { get; }

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
                DeckBuilderViewModel.GetInstance().AddCards(null, true);
            }
        }

        private void ClearDeck(object _)
        {
            if (this.HandleRequestSave())
            {
                this.SaveFile = string.Empty;
                DeckBuilderViewModel.GetInstance().AddCards(null, true);
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
                    DeckBuilderViewModel.GetInstance().AddCards(this._deckController.LoadDeckFromFile(openFileDialog.FileName).Cast<FullCard>(), true);
                    this.SaveFile = openFileDialog.FileName;
                }
            }
        }

        private bool HandleRequestSave()
        {
            // Till save is implemented...
            return true;

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

        private void SaveDeck(object commandParameter)
        {
            if (commandParameter is bool saveAs)
            {
                this.HandleSave(saveAs);
            }
        }

        private async void ImportDeck(object commandParameter)
        {
            if (commandParameter is string importType && this.HandleRequestSave())
            {
                bool overrideDeck = false;
                if (DeckBuilderViewModel.GetInstance().DeckCards.Count > 0)
                {
                    switch (this.ShowQuestion("Import", "Do you want to override your Deck?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, "Save Deck & Override", "Discard Deck & Override", "Add Cards"))
                    {
                        case MessageDialogResult.Canceled:
                            return;
                        case MessageDialogResult.Negative:
                            overrideDeck = true;
                            break;
                        case MessageDialogResult.Affirmative:
                            // Till save is implemented...
                            //this.HandleSave(false);
                            overrideDeck = true;
                            break;
                        case MessageDialogResult.FirstAuxiliary:
                            overrideDeck = false;
                            break;
                    }
                }

                string importCards = string.Empty;
                if (importType.Equals("TEXT", StringComparison.OrdinalIgnoreCase))
                {
                    importCards = await CopyShopView.GetInstance().ShowChildWindowAsync<string>(new ImportCardsChildView() { IsModal = false }).ConfigureAwait(false);
                }
                else if (importType.Equals("CLIPBOARD", StringComparison.OrdinalIgnoreCase))
                {
                    importCards = Clipboard.GetText();
                }
                else
                {
                    return;
                }

                this.ShowProgress("IMPORT", "Please wait while your deck is imported...", new Action(() => this.ImportDeckCards(importCards, overrideDeck)));
            }
        }

        private void ImportDeckCards(string cards, bool overrideDeck)
        {
            List<IFullCard> importedCards = this._cardController.GetCardsByNameList(cards ?? "");
            this.SendErrorMessage(this._cardController.ErrorMessage);
            if (importedCards.Count > 0)
            {
                DeckBuilderViewModel.GetInstance().AddCards(importedCards.ConvertAll(card => new FullCard(card)), overrideDeck);
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

        private void OpenLink(object commandParameter)
        {
            if (commandParameter is string link && Uri.TryCreate(link, UriKind.Absolute, out Uri hyperlink))
            {
                Process.Start(new ProcessStartInfo(hyperlink.AbsoluteUri));
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

        internal MessageDialogResult ShowQuestion(string header, string message, MessageDialogStyle style,
            string affirmativeButtonText = "YES", string negativeButtonText = "NO", string firstAuxiliaryButtonText = "CANCEL", string secondAuxiliaryButtonText = "")
            => CopyShopView.GetInstance().ShowModalMessageExternal(header, message, style,
                 new MetroDialogSettings()
                 {
                     AffirmativeButtonText = affirmativeButtonText,
                     NegativeButtonText = negativeButtonText,
                     FirstAuxiliaryButtonText = firstAuxiliaryButtonText,
                     SecondAuxiliaryButtonText = secondAuxiliaryButtonText,
                     DialogResultOnCancel = MessageDialogResult.Canceled,
                     DefaultButtonFocus = MessageDialogResult.Affirmative
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
