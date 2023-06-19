using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;

using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Fontend.WPF.Base;
using Tolarian.Copyshop.Fontend.WPF.Communication;
using Tolarian.Copyshop.Fontend.WPF.Helper;
using Tolarian.Copyshop.Fontend.WPF.Model;
using Tolarian.Copyshop.Fontend.WPF.Views;

using static Tolarian.Copyshop.Business.UseCaseInteractors.PrintInteractor;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class DeckPrintViewModel : BindableBase
    {
        #region fields

        private static DeckPrintViewModel _deckPrintViewModel;
        private readonly CardController _cardController;
        private readonly DeckCardModel _deckCardModel;
        private readonly PrintController _printController;
        private readonly Dialogs _dialogs;

        private int _cardScale = 100;
        private PageFormat _selectedPageFormat;

        #endregion

        #region ctor

        public DeckPrintViewModel(CardController cardController, PrintController printController, DeckCardModel deckCardModel, Dialogs dialogs)
        {
            _deckPrintViewModel = this;
            _cardController = cardController;
            _deckCardModel = deckCardModel;
            _printController = printController;
            _dialogs = dialogs;

            PrintCommand = new Command(OnPrintCommand);
            ResetCardScaleCommand = new Command(OnResetCardScaleCommand);

            SelectedPageFormat = PageFormat.A4;
        }

        #endregion

        #region Properties

        public Command PrintCommand { get; set; }

        public Command ResetCardScaleCommand { get; set; }

        public int CardScale
        {
            get => _cardScale;
            set
            {
                SetProperty(ref _cardScale, value);
                DeckPrintView.GetInstance()?.ReloadDocumentPreview();
            }
        }

        public IEnumerable<PageFormat> PageFormats
            => Enum.GetValues(typeof(PageFormat)).Cast<PageFormat>();

        public PageFormat SelectedPageFormat
        {
            get => _selectedPageFormat;
            set
            {
                SetProperty(ref _selectedPageFormat, value);
                DeckPrintView.GetInstance()?.ReloadDocumentPreview();
            }
        }


        public ObservableCollection<FullCardModel> DeckCards
        {
            get => _deckCardModel.DeckCards;
            set
            {
                if (!Equals(_deckCardModel.DeckCards, value))
                {
                    _deckCardModel.DeckCards = value;
                    OnPropertyChanged(nameof(DeckCards));
                    DeckBuilderViewModel.GetInstance().InvokeDeckCards();
                }
            }
        }

        #endregion

        #region static methods

        public static DeckPrintViewModel GetInstance()
            => _deckPrintViewModel;

        #endregion

        #region public methods

        public void InvokeDeckCards()
            => OnPropertyChanged(nameof(DeckCards));

        public FixedDocument GetPrintPages()
            => _printController.GetPrintPages(SelectedPageFormat, DeckCards.Cast<IFullCard>().ToList(), (float)CardScale / 100);

        #endregion

        #region private methods

        private void OnPrintCommand(object _)
            => new TolorianDeckPrinterHelper(DeckPrintView.GetInstance().PrintDocumentPreview.Document, _dialogs).Print();

        private void OnResetCardScaleCommand(object _)
            => CardScale = 100;

        #endregion

    }
}