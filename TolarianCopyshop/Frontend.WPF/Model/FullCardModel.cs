using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.Model
{
    public class FullCardModel : BindableBase, IFullCard
    {
        #region Fields

        private Guid _cardId;
        private int _cardCount = 1;
        private string _formattedCardName;
        private Dictionary<string, string> _legalities1;
        private Dictionary<string, string> _legalities2;
        private Guid _printId;
        private string _setCode;
        private ICollection<CardFace> _cardFaces;
        private bool _hasArtworks = true;
        private bool _isTransformable;
        private Uri _selectedCardFace;

        #endregion

        #region Create

        public static FullCardModel Create(IFullCard card)
        {
            if (card is null)
            {
                return new FullCardModel();
            }

            return new FullCardModel
            {
                CardCount = card.CardCount,
                CardId = card.CardId,
                PrintId = card.PrintId,
                FormattedCardName = card.FormattedCardName,
                Legalities1 = card.Legalities1,
                Legalities2 = card.Legalities2,
                SetCode = card.SetCode,
                CardFaces = card.CardFaces,
                IsTransformable = card.IsTransformable,
                HasArtworks = true,
            };
        }

        #endregion

        #region Properties

        public Guid CardId
        {
            get => this._cardId;
            set => this.SetProperty(ref this._cardId, value);
        }

        public Guid PrintId
        {
            get => this._printId;
            set => this.SetProperty(ref this._printId, value);
        }

        public string FormattedCardName
        {
            get => this._formattedCardName;
            set => this.SetProperty(ref this._formattedCardName, value);
        }

        public int CardCount
        {
            get => this._cardCount;
            set => this.SetProperty(ref this._cardCount, value);
        }

        public Dictionary<string, string> Legalities1
        {
            get => this._legalities1;
            set => this.SetProperty(ref this._legalities1, value);
        }

        public Dictionary<string, string> Legalities2
        {
            get => this._legalities2;
            set => this.SetProperty(ref this._legalities2, value);
        }

        public string SetCode
        {
            get => this._setCode;
            set => this.SetProperty(ref this._setCode, value);
        }

        public ICollection<CardFace> CardFaces
        {
            get => this._cardFaces;
            set => this.SetProperty(ref this._cardFaces, value);
        }

        public bool IsTransformable
        {
            get => this._isTransformable;
            set => this.SetProperty(ref this._isTransformable, value);
        }

        public bool HasArtworks
        {
            get => this._hasArtworks;
            set => this.SetProperty(ref this._hasArtworks, value);
        }

        public Uri SelectedCardFace
        {
            get => this._selectedCardFace;
            set => this.SetProperty(ref this._selectedCardFace, value);
        }

        #endregion

        #region Convert

        public static implicit operator FullCard(FullCardModel card)
            => FullCard.Create(card);

        public static implicit operator FullCardModel(FullCard card)
            => FullCardModel.Create(card);

        #endregion
    }
}
