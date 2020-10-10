using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.ScreenPresenter.Base;
using Tolarian.Copyshop.ScreenPresenter.Communication;
using Tolarian.Copyshop.ScreenPresenter.Model;
using Tolarian.Copyshop.ScreenPresenter.Views;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class CopyShopViewModel : BindableBase
    {
        #region Fields

        private static CopyShopViewModel _copyshop;
        private readonly CardController _cardController;
        private readonly DeckController _deckController;
        private readonly PrintController _printController;
        private readonly Dialogs _dialogs;

        #endregion

        #region Constructor

        public CopyShopViewModel(CardController cardController, PrintController printController, DeckController deckController, Dialogs dialogs)
        {
            _copyshop = this;
            this._cardController = cardController;
            this._printController = printController;
            this._deckController = deckController;
            this._dialogs = dialogs;

            // Commands
            this.NewCommand = new Command(this.NewDeck);
            this.OpenCommand = new Command(this.OpenDeck);
            this.SaveCommand = new Command(this.SaveDeck);
            this.ImportCommand = new Command(this.ImportDeck);
            this.ClearCommand = new Command(this.ClearDeck);
            this.ImportTokenCommand = new Command(this.ImportToken);
            this.OpenLinkCommand = new Command(this.OpenLink);
        }

        #endregion

        #region Properties

        public Command NewCommand { get; }

        public Command OpenCommand { get; }

        public Command SaveCommand { get; }

        public Command ImportCommand { get; }

        public Command ImportTokenCommand { get; }

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
                    DefaultExt = ".tcd",
                    Filter = "Tolarian Copyshop Deck (*.tcd)|*.tcd|All files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),

                };
                if (openFileDialog.ShowDialog() == true)
                {
                    this._dialogs.ShowProgressOnUIThread("Loading Deck", "Please wait while your deck is loaded...", new Action(() =>
                    {
                        System.Collections.Generic.List<FullCardModel> response = this._deckController.LoadDeckFromFile(openFileDialog.FileName).ConvertAll(FullCardModel.Create);

                        this._dialogs.SendErrorMessage(this._cardController.GetErrorMessage());
                        if (response.Count > 0)
                        {
                            DeckBuilderViewModel.GetInstance().AddCards(response, true);
                        }
                    }));
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
            switch (this._dialogs.ShowQuestionOnUIThread("Save", "Do you want to save your Deck?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary))
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
                DefaultExt = ".tcd",
                Filter = "Tolarian Copyshop Deck (*.tcd)|*.tcd|All files (*.*)|*.*",
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

        private void ImportToken(object _)
        {
            bool replaceTokens = false;
            if (DeckBuilderViewModel.GetInstance().DeckCards.Count > 0)
            {
                switch (this._dialogs.ShowQuestionOnUIThread("Import Tokens", "Do you want to remove exiting tokens in your deck?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary))
                {
                    case MessageDialogResult.FirstAuxiliary:
                    case MessageDialogResult.SecondAuxiliary:
                    case MessageDialogResult.Canceled:
                        return;
                    case MessageDialogResult.Negative:
                        replaceTokens = false;
                        break;
                    case MessageDialogResult.Affirmative:
                        replaceTokens = true;
                        break;
                }
            }

            this._dialogs.ShowProgressOnUIThread("IMPORT TOKEN", "Please wait while your tokens are imported...", new Action(() => this.ImportTokenCards(replaceTokens)));
        }

        private void ImportTokenCards(bool replaceTokens)
        {
            Controller.ResponseObjects.AddTokensToDeckResponse response = this._cardController.AddTokensToDeck(DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList(), replaceTokens);
            this._dialogs.SendErrorMessage(this._cardController.GetErrorMessage());
            if (response.Deck.Count > 0)
            {
                DeckBuilderViewModel.GetInstance().AddCards(response.Deck.ConvertAll(FullCardModel.Create), true);
            }
        }

        private async void ImportDeck(object commandParameter)
        {
            if (commandParameter is string importType)
            {
                bool overrideDeck = false;
                if (DeckBuilderViewModel.GetInstance().DeckCards.Count > 0)
                {
                    switch (this._dialogs.ShowQuestionOnUIThread("Import", "Do you want to override your Deck?", MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary, "Save Deck & Override", "Discard Deck & Override", "Add Cards", "Cancel"))
                    {
                        case MessageDialogResult.SecondAuxiliary:
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
                    importCards = await this._dialogs.ShowChildWindowOnUIThread<string>(new ImportCardsChildView()).ConfigureAwait(false);
                }
                else if (importType.Equals("CLIPBOARD", StringComparison.OrdinalIgnoreCase))
                {
                    importCards = Clipboard.GetText();
                }
                else
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(importCards))
                {
                    string notFoundCards = string.Empty;
                    this._dialogs.ShowProgressOnUIThread("IMPORT", "Please wait while your deck is imported...",
                        new Action(() => this.ImportDeckCards(importCards, overrideDeck, ref notFoundCards)),
                        new Action(() => this.Post_ImportDeckCards(ref notFoundCards)));
                }
            }
        }

        private void ImportDeckCards(string cards, bool overrideDeck, ref string notFoundCards)
        {
            Controller.ResponseObjects.CardImportResponse response = this._cardController.GetCardsByImportString(cards ?? "");
            this._dialogs.SendErrorMessage(this._cardController.GetErrorMessage());
            if (response.Cards.Count > 0)
            {
                DeckBuilderViewModel.GetInstance().AddCards(response.Cards.ConvertAll(FullCardModel.Create), overrideDeck);
            }
            notFoundCards = response.NotFound;
        }

        private void Post_ImportDeckCards(ref string notFoundCards)
        {
            if (!string.IsNullOrWhiteSpace(notFoundCards))
            {
                this._dialogs.ShowMessageOnUIThread("Missing Cards", "The following cards could not be found:" + Environment.NewLine + notFoundCards);
            }
        }

        private void OpenLink(object commandParameter)
        {
            if (commandParameter is string link && Uri.TryCreate(link, UriKind.Absolute, out Uri hyperlink))
            {
                _ = Process.Start(new ProcessStartInfo(hyperlink.AbsoluteUri));
            }
        }

        #endregion

    }
}
