using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.Mappers;

namespace Tolarian.Copyshop.Controller
{
    public class DeckController : TolarianControllerBase
    {
        private readonly IDeckInfoInteractor _deckInfoInteractor;

        public DeckController(IDeckInfoInteractor deckInfoInteractor)
        {
            this._deckInfoInteractor = deckInfoInteractor;
        }

        public List<IFullCard> LoadDeckFromFile(string fileName)
            => throw new NotImplementedException();

        public bool SaveDeckToFile(string fileName, List<IFullCard> deckCards)
            => throw new NotImplementedException();

        public int GetTotalCardCountOfDeck(List<IFullCard> deckCards)
        {
            List<DeckInfoCard> businessModel =  DeckMapper.MapDeckDtoToBusiness(deckCards);
            return _deckInfoInteractor.GetTotalCardCountOfDeck(businessModel);
        }
    }
}
