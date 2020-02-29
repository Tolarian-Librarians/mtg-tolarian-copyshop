using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller
{
    public class DeckController : TolarianControllerBase
    {
        private readonly IDeckInfoInteractor _deckInfoInteractor;
        private readonly IMapper _mapper;

        public DeckController(IDeckInfoInteractor deckInfoInteractor, IMapper mapper)
        {
            this._deckInfoInteractor = deckInfoInteractor;
            this._mapper = mapper;
        }

        public List<IFullCard> LoadDeckFromFile(string fileName)
            => throw new NotImplementedException();

        public bool SaveDeckToFile(string fileName, List<IFullCard> deckCards)
            => throw new NotImplementedException();

        public int GetTotalCardCountOfDeck(List<IFullCard> deckCards)
        {
            List<DeckInfoCard> businessModel = _mapper.Map<List<DeckInfoCard>>(deckCards);
            return _deckInfoInteractor.GetTotalCardCountOfDeck(businessModel);
        }
    }
}
