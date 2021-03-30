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
            this._cardController = cardController;
            this._deckCardModel = deckCardModel;
            this._printController = printController;
            this._dialogs = dialogs;

            this.PrintCommand = new Command(this.OnPrintCommand);
            this.ResetCardScaleCommand = new Command(this.OnResetCardScaleCommand);

            this.SelectedPageFormat = PageFormat.A4;
        }

        #endregion

        #region Properties

        public Command PrintCommand { get; set; }

        public Command ResetCardScaleCommand { get; set; }

        public int CardScale
        {
            get => this._cardScale;
            set
            {
                this.SetProperty(ref this._cardScale, value);
                DeckPrintView.GetInstance()?.ReloadDocumentPreview();
            }
        }

        public IEnumerable<PageFormat> PageFormats 
            => Enum.GetValues(typeof(PageFormat)).Cast<PageFormat>();

        public PageFormat SelectedPageFormat
        {
            get => this._selectedPageFormat;
            set
            {
                this.SetProperty(ref this._selectedPageFormat, value);
                DeckPrintView.GetInstance()?.ReloadDocumentPreview();
            }
        }


        public ObservableCollection<FullCardModel> DeckCards
        {
            get => this._deckCardModel.DeckCards;
            set
            {
                if (!Equals(this._deckCardModel.DeckCards, value))
                {
                    this._deckCardModel.DeckCards = value;
                    this.OnPropertyChanged(nameof(this.DeckCards));
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
            => this.OnPropertyChanged(nameof(this.DeckCards));

        public FixedDocument GetPrintPages()
            => this._printController.GetPrintPages(this.SelectedPageFormat, this.DeckCards.Cast<IFullCard>().ToList(), (float)this.CardScale / 100);

        #endregion

        #region private methods

        private void OnPrintCommand(object _)
            => new TolorianDeckPrinterHelper(DeckPrintView.GetInstance().PrintDocumentPreview.Document, this._dialogs).Print();

        private void OnResetCardScaleCommand(object _)
            => this.CardScale = 100;

        #endregion

    }
}
