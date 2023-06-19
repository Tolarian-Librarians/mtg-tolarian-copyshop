using System;
using System.Collections.Generic;

using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;
using Tolarian.Copyshop.Fontend.WPF.Base;

namespace Tolarian.Copyshop.Fontend.WPF.Model
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
            get => _cardId;
            set => SetProperty(ref _cardId, value);
        }

        public float ConvertedManaCost
        {
            get => _convertedManaCost;
            set => SetProperty(ref _convertedManaCost, value);
        }

        public List<MtgColor> ColorIdentity
        {
            get => _colorIdentity;
            set => SetProperty(ref _colorIdentity, value);
        }

        public List<MtgColor> Colors
        {
            get => _colors;
            set => SetProperty(ref _colors, value);
        }

        public List<MtgColor> ProducedMana
        {
            get => _producedMana;
            set => SetProperty(ref _producedMana, value);
        }

        public Guid PrintId
        {
            get => _printId;
            set => SetProperty(ref _printId, value);
        }

        public string FormattedCardName
        {
            get => _formattedCardName;
            set => SetProperty(ref _formattedCardName, value);
        }

        public string ManaCostLine
        {
            get => _manaCostLine;
            set => SetProperty(ref _manaCostLine, value);
        }

        public int CardCount
        {
            get => _cardCount;
            set => SetProperty(ref _cardCount, value);
        }

        public Dictionary<string, string> Legalities
        {
            get => _legalities;
            set => SetProperty(ref _legalities, value);
        }

        public string SetCode
        {
            get => _setCode;
            set => SetProperty(ref _setCode, value);
        }

        public ICollection<CardFace> CardFaces
        {
            get => _cardFaces;
            set => SetProperty(ref _cardFaces, value);
        }

        public ICollection<RelatedCard> RelatedCards
        {
            get => _relatedCards;
            set => SetProperty(ref _relatedCards, value);
        }

        public bool IsTransformable
        {
            get => _isTransformable;
            set => SetProperty(ref _isTransformable, value);
        }

        public bool HasArtworks
        {
            get => _hasArtworks;
            set => SetProperty(ref _hasArtworks, value);
        }

        public Uri SelectedCardFace
        {
            get => _selectedCardFace;
            set => SetProperty(ref _selectedCardFace, value);
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