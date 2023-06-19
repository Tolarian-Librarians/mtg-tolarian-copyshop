using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Win32;

using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Fontend.WPF.Base;
using Tolarian.Copyshop.Fontend.WPF.Communication;
using Tolarian.Copyshop.Fontend.WPF.Model;
using Tolarian.Copyshop.Fontend.WPF.Views;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class CopyShopViewModel : BindableBase
    {
        #region Fields

        private static CopyShopViewModel _copyshop;
        private readonly CardController _cardController;
        private readonly DeckController _deckController;
        private readonly ExportController _exportController;
        private readonly Dialogs _dialogs;

        private int _selectedTabIndex;

        #endregion

        #region Constructor

        public CopyShopViewModel(CardController cardController,
                                 DeckController deckController,
                                 ExportController exportController,
                                 Dialogs dialogs)
        {
            _copyshop = this;
            _cardController = cardController;
            _deckController = deckController;
            _exportController = exportController;
            _dialogs = dialogs;

            // Commands
            NewCommand = new Command(NewDeck);
            OpenCommand = new Command(OpenDeck);
            SaveCommand = new Command(SaveDeck);
            ImportCommand = new Command(ImportDeck);
            ExportCommand = new Command(ExportDeck);
            ClearCommand = new Command(ClearDeck);
            ImportTokenCommand = new Command(ImportToken);
            OpenLinkCommand = new Command(OpenLink);
        }

        #endregion

        #region Properties

        public Command NewCommand { get; }

        public Command OpenCommand { get; }

        public Command SaveCommand { get; }

        public Command ImportCommand { get; }

        public Command ExportCommand { get; }

        public Command ImportTokenCommand { get; }

        public Command PrintCommand { get; }

        public Command ClearCommand { get; }

        public Command OpenLinkCommand { get; }

        public string SaveFile { get; set; }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        #endregion

        #region Methods

        internal static CopyShopViewModel GetInstance()
            => _copyshop;

        private void NewDeck(object _)
            => ClearDeck(_);

        private void ClearDeck(object _)
        {
            if (HandleRequestSave())
            {
                SaveFile = string.Empty;
                DeckBuilderViewModel.GetInstance().AddCards(null, true);
                GoToTabPage(0);
            }
        }

        private void OpenDeck(object _)
        {
            if (HandleRequestSave())
            {
                OpenFileDialog openFileDialog = new()
                {
                    AddExtension = true,
                    DefaultExt = ".tcd",
                    Filter = "Tolarian Copyshop Deck (*.tcd)|*.tcd|All files (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),

                };
                if (openFileDialog.ShowDialog() == true)
                {
                    _dialogs.ShowProgressOnUIThread("Loading Deck", "Please wait while your deck is loaded...",
                        new Action(() =>
                        {
                            List<FullCardModel> response = _deckController.LoadDeckFromFile(openFileDialog.FileName).ConvertAll(FullCardModel.Create);

                            _dialogs.SendErrorMessage(_cardController.GetErrorMessage());
                            if (response.Count > 0)
                            {
                                DeckBuilderViewModel.GetInstance().AddCards(response, true);
                            }
                        }),
                        new Action(() => GoToTabPage(0)));
                    SaveFile = openFileDialog.FileName;
                }
            }
        }

        private bool HandleRequestSave()
        {

            if (DeckBuilderViewModel.GetInstance().DeckCards.Count == 0)
            {
                return true;
            }
            return _dialogs.ShowQuestionOnUIThread("Save", "Do you want to save your Deck?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary) switch
            {
                MessageDialogResult.Canceled or MessageDialogResult.FirstAuxiliary or MessageDialogResult.SecondAuxiliary => false,
                MessageDialogResult.Affirmative => HandleSave(false),
                _ => true,
            };
        }

        private bool HandleSave(bool saveAs)
        {
            if (!saveAs && !string.IsNullOrEmpty(SaveFile))
            {
                return _deckController.SaveDeckToFile(SaveFile, DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());
            }

            SaveFileDialog saveFileDialog = new()
            {
                AddExtension = true,
                DefaultExt = ".tcd",
                Filter = "Tolarian Copyshop Deck (*.tcd)|*.tcd|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveFile = saveFileDialog.FileName;
                return _deckController.SaveDeckToFile(saveFileDialog.FileName, DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());
            }

            return false;
        }

        private void SaveDeck(object commandParameter)
        {
            if (commandParameter is bool saveAs)
            {
                HandleSave(saveAs);
            }
        }

        private void ImportToken(object _)
        {
            bool replaceTokens = false;
            if (DeckBuilderViewModel.GetInstance().DeckCards.Count > 0)
            {
                switch (_dialogs.ShowQuestionOnUIThread("Import Tokens", "Do you want to remove exiting tokens in your deck?", MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary))
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

            _dialogs.ShowProgressOnUIThread("IMPORT TOKEN", "Please wait while your tokens are imported...", new Action(() => ImportTokenCards(replaceTokens)));
            GoToTabPage(0);
        }

        private void ImportTokenCards(bool replaceTokens)
        {
            AddTokensToDeckResponse response = _cardController.AddTokensToDeck(DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList(), replaceTokens);
            _dialogs.SendErrorMessage(_cardController.GetErrorMessage());
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
                    switch (_dialogs.ShowQuestionOnUIThread("Import", "Do you want to override your Deck?", MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary, "Save Deck & Override", "Discard Deck & Override", "Add Cards", "Cancel"))
                    {
                        case MessageDialogResult.SecondAuxiliary:
                        case MessageDialogResult.Canceled:
                            return;
                        case MessageDialogResult.Negative:
                            overrideDeck = true;
                            break;
                        case MessageDialogResult.Affirmative:
                            HandleSave(false);
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
                    importCards = await _dialogs.ShowChildWindowOnUIThread<string>(new MultiLineInputChildView("IMPORT DECK", string.Empty)).ConfigureAwait(false);
                }
                else if (importType.Equals("CLIPBOARD", StringComparison.OrdinalIgnoreCase))
                {
                    importCards = Clipboard.GetText();
                }
                else if (importType.Equals("URL", StringComparison.OrdinalIgnoreCase))
                {
                    await ImportDeckByUrl(overrideDeck).ConfigureAwait(false);
                    return;
                }
                else
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(importCards))
                {
                    string notFoundCards = string.Empty;
                    _dialogs.ShowProgressOnUIThread("IMPORT", "Please wait while your deck is imported...",
                        new Action(() => ImportDeckCardsByString(importCards, overrideDeck, ref notFoundCards)),
                        new Action(() => Post_ImportDeckCards(ref notFoundCards)));
                }
            }
        }

        private async Task ImportDeckByUrl(bool overrideDeck)
        {
            string Url = await _dialogs.ShowInputOnUIThread("IMPORT BY TAPPEDOUT.NET", "Please enter URL...").ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(Url))
            {
                string notFoundCards = string.Empty;
                _dialogs.ShowProgressOnUIThread("IMPORT", "Please wait while your deck is imported...",
                    new Action(() => ImportDeckCardsByUrl(Url, overrideDeck, ref notFoundCards)),
                    new Action(() => Post_ImportDeckCards(ref notFoundCards)));
            }
        }

        private void ImportDeckCardsByUrl(string url, bool overrideDeck, ref string notFoundCards)
        {
            CardImportResponse response = _cardController.GetCardsFromUri(url ?? "");
            ImportDeckCards(response, overrideDeck, ref notFoundCards);
        }

        private void ImportDeckCardsByString(string cards, bool overrideDeck, ref string notFoundCards)
        {
            CardImportResponse response = _cardController.GetCardsByImportString(cards ?? "");
            ImportDeckCards(response, overrideDeck, ref notFoundCards);
        }

        private void ImportDeckCards(CardImportResponse response, bool overrideDeck, ref string notFoundCards)
        {
            _dialogs.SendErrorMessage(_cardController.GetErrorMessage());
            if (response.Cards.Count > 0)
            {
                DeckBuilderViewModel.GetInstance().AddCards(response.Cards.ConvertAll(FullCardModel.Create), overrideDeck);
            }
            notFoundCards = response.NotFound;
        }

        private void ExportDeck(object commandParameter)
        {
            if (commandParameter is string importType)
            {
                string cards = _exportController.ExportDeck(DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());
                if (importType.Equals("TEXT", StringComparison.OrdinalIgnoreCase))
                {
                    _dialogs.ShowChildWindowOnUIThread<string>(new MultiLineInputChildView("EXPORT DECK", cards));
                }
                else if (importType.Equals("CLIPBOARD", StringComparison.OrdinalIgnoreCase))
                {
                    Clipboard.SetText(cards);
                }
            }
        }

        private void Post_ImportDeckCards(ref string notFoundCards)
        {
            if (!string.IsNullOrWhiteSpace(notFoundCards))
            {
                GoToTabPage(0);
                _dialogs.ShowMessageOnUIThread("Missing Cards", "The following cards could not be found:" + Environment.NewLine + notFoundCards);
            }
        }

        private void OpenLink(object commandParameter)
        {
            if (commandParameter is string link && Uri.TryCreate(link, UriKind.Absolute, out Uri hyperlink))
            {
                _ = Process.Start(new ProcessStartInfo(hyperlink.AbsoluteUri));
            }
        }

        private void GoToTabPage(int page)
            => SelectedTabIndex = page;

        #endregion

    }
}