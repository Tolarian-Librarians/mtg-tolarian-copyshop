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
        private readonly ISaveAndLoadInteractor _saveAndLoadInteractor;

        public DeckController(IDeckInfoInteractor deckInfoInteractor, ISaveAndLoadInteractor saveAndLoadInteractor)
        {
            this._deckInfoInteractor = deckInfoInteractor;
            this._saveAndLoadInteractor = saveAndLoadInteractor;
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
            catch(Exception ex)
            {
                SetErrorMessage(ex);
                return false;
            }
        }

        public int GetTotalCardCountOfDeck(List<IFullCard> deckCards)
        {
            List<DeckInfoCard> businessModel =  DeckMapper.MapDeckDtoToBusiness(deckCards);
            return _deckInfoInteractor.GetTotalCardCountOfDeck(businessModel);
        }
    }
}
