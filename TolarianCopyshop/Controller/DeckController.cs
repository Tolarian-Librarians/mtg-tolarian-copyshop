using System;
using System.Collections.Generic;
using System.Linq;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.Mappers;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller
{
    public class DeckController : TolarianControllerBase
    {
        private readonly IDeckInfoInteractor _deckInfoInteractor;
        private readonly ISaveAndLoadInteractor _saveAndLoadInteractor;

        public DeckController(IDeckInfoInteractor deckInfoInteractor, ISaveAndLoadInteractor saveAndLoadInteractor)
        {
            _deckInfoInteractor = deckInfoInteractor;
            _saveAndLoadInteractor = saveAndLoadInteractor;
        }

        public List<IFullCard> LoadDeckFromFile(string fileName)
        {
            var response = new List<IFullCard>();
            try
            {
                var businessResponse = _saveAndLoadInteractor.LoadDeck(fileName);
                response = CardMapper.MapToCardDto(businessResponse);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
            }

            return response;
        }

        public bool SaveDeckToFile(string fileName, List<IFullCard> deckCards)
        {
            try
            {
                _saveAndLoadInteractor.SaveDeck(SaveAndLoadMapper.ConvertToBusiness(deckCards), fileName);
                return true;
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
                return false;
            }
        }

        public int GetTotalCardCountOfDeck(List<IFullCard> deckCards)
        {
            List<DeckInfoCard> businessModel = DeckMapper.MapDeckDtoToBusiness(deckCards);
            return _deckInfoInteractor.GetTotalCardCountOfDeck(businessModel);
        }

        public GetDeckStatisticsResponse GetDeckStatistics(List<IFullCard> deckCards)
        {
            if (deckCards == null)
                return GetDeckStatisticsResponse.Empty();

            var businessModel = DeckMapper.MapDeckDtoToBusiness(deckCards);

            var result = new GetDeckStatisticsResponse
            {
                CardTypeCounts = _deckInfoInteractor.GetCardTypeCounts(businessModel).ToDictionary(x => (CardType)((int)x.Key), x => x.Value),
                ColorCardsCounts = _deckInfoInteractor.GetColorCardCounts(businessModel).ToDictionary(x => (MtgColor)((int)x.Key), x => x.Value),
                ColorSymbolCounts = _deckInfoInteractor.GetColorSymbolCounts(businessModel).ToDictionary(x => (MtgColor)((int)x.Key), x => x.Value),
                ManaSourcesCounts = _deckInfoInteractor.GetManaSourcesCounts(businessModel).ToDictionary(x => (MtgColor)((int)x.Key), x => x.Value),
                ManaCurveCreatures = _deckInfoInteractor.GetCreatureManaCurve(businessModel),
                ManaCurveNonCreatures = _deckInfoInteractor.GetNonCreatureManaCurve(businessModel),
                TotalCards = _deckInfoInteractor.GetTotalCardCountOfDeck(businessModel),
                AverageCmc = _deckInfoInteractor.GetAverageCmc(businessModel),
                CreatureCount = _deckInfoInteractor.GetCreatureCount(businessModel),
                NonCreatureCount = _deckInfoInteractor.GetNonCreatureCount(businessModel),
            };

            return result;
        }
    }
}