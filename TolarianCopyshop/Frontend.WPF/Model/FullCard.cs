using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.Model
{
    public class FullCard : BindableBase, IFullCard
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

        #endregion

        #region ctor

        public FullCard()
        {

        }

        public FullCard(IFullCard card)
        {
            if (card != null)
            {
                this.CardType = card.CardType;
                this.CardCount = card.CardCount;
                this.Id = card.Id;
                this.Legalities1 = card.Legalities1;
                this.Legalities2 = card.Legalities2;
                this.Name = card.Name;
                this.LargeImage = card.LargeImage;
                this.SmallImage = card.SmallImage;
                this.Text = card.Text;
                this.SetCode = card.SetCode;
            }
        }

        #endregion

        #region Properties

        public CardType CardType
        {
            get => this._cardType;
            set => this.SetProperty(ref this._cardType, value);
        }

        public Guid Id
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
        public string SetCode { get; set; }

        #endregion

        #region Convert

        public static implicit operator FullCardResponse(FullCard card)
            => new FullCardResponse(card);

        public static implicit operator FullCard(FullCardResponse card)
            => new FullCard(card);

        #endregion
    }
}
