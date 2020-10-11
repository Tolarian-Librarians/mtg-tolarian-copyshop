using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.Model
{
    public class FullCardModel : BindableBase, IFullCard
    {
        #region Fields

        private Guid _cardId;
        private int _cardCount = 1;
        private string _formattedCardName;
        private Dictionary<string, string> _legalities;
        private Guid _printId;
        private string _setCode;
        private string _manaCostLine;
        private ICollection<CardFace> _cardFaces;
        private ICollection<RelatedCard> _relatedCards;
        private bool _hasArtworks = true;
        private bool _isTransformable;
        private Uri _selectedCardFace;
        private float _convertedManaCost;
        private List<MtgColor> _colorIdentity;
        private List<MtgColor> _colors;
        private List<MtgColor> _producedMana;

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
                Legalities = card.Legalities,
                SetCode = card.SetCode,
                CardFaces = card.CardFaces,
                RelatedCards = card.RelatedCards,
                IsTransformable = card.IsTransformable,
                HasArtworks = true,
                ConvertedManaCost = card.ConvertedManaCost,
                ColorIdentity = card.ColorIdentity,
                Colors = card.Colors,
                ManaCostLine = card.ManaCostLine,
                ProducedMana = card.ProducedMana,
            };
        }

        #endregion

        #region Properties

        public Guid CardId
        {
            get => this._cardId;
            set => this.SetProperty(ref this._cardId, value);
        }

        public float ConvertedManaCost
        {
            get => this._convertedManaCost;
            set => this.SetProperty(ref this._convertedManaCost, value);
        }

        public List<MtgColor> ColorIdentity
        {
            get => this._colorIdentity;
            set => this.SetProperty(ref this._colorIdentity, value);
        }

        public List<MtgColor> Colors
        {
            get => this._colors;
            set => this.SetProperty(ref this._colors, value);
        }

        public List<MtgColor> ProducedMana
        {
            get => this._producedMana;
            set => this.SetProperty(ref this._producedMana, value);
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

        public string ManaCostLine
        {
            get => this._manaCostLine;
            set => this.SetProperty(ref this._manaCostLine, value);
        }

        public int CardCount
        {
            get => this._cardCount;
            set => this.SetProperty(ref this._cardCount, value);
        }

        public Dictionary<string, string> Legalities
        {
            get => this._legalities;
            set => this.SetProperty(ref this._legalities, value);
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

        public ICollection<RelatedCard> RelatedCards
        {
            get => this._relatedCards;
            set => this.SetProperty(ref this._relatedCards, value);
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
