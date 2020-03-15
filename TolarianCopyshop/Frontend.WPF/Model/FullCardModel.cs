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

        private CardType _cardType;
        private Guid _id;
        private int _cardCount = 1;
        private Dictionary<string, string> _legalities1;
        private Dictionary<string, string> _legalities2;
        private string _name;
        private Uri _pngImage;
        private Uri _smallImage;
        private string _text;
        private Uri _croppedImage;
        private Guid _printId;
        private string _setCode;

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
                CardType = card.CardType,
                CardCount = card.CardCount,
                CardId = card.CardId,
                PrintId = card.PrintId,
                Legalities1 = card.Legalities1,
                Legalities2 = card.Legalities2,
                Name = card.Name,
                LargeImage = card.LargeImage,
                SmallImage = card.SmallImage,
                Text = card.Text,
                SetCode = card.SetCode,
                CroppedImage = card.CroppedImage,
            };
        }

        #endregion

        #region Properties

        public CardType CardType
        {
            get => this._cardType;
            set => this.SetProperty(ref this._cardType, value);
        }

        public Guid CardId
        {
            get => this._id;
            set => this.SetProperty(ref this._id, value);
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

        public string Name
        {
            get => this._name;
            set => this.SetProperty(ref this._name, value);
        }

        public Uri LargeImage
        {
            get => this._pngImage;
            set => this.SetProperty(ref this._pngImage, value);
        }

        public Uri SmallImage
        {
            get => this._smallImage;
            set => this.SetProperty(ref this._smallImage, value);
        }

        public string Text
        {
            get => this._text;
            set => this.SetProperty(ref this._text, value);
        }

        public string SetCode
        {
            get => this._setCode;
            set => this.SetProperty(ref this._setCode, value);
        }

        public Guid PrintId
        {
            get => this._printId;
            set => this.SetProperty(ref this._printId, value);
        }

        public Uri CroppedImage
        {
            get => this._croppedImage;
            set => this.SetProperty(ref this._croppedImage, value);
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
