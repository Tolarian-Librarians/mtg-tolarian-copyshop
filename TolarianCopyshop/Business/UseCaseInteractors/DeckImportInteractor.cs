using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Business.Models.SfSetInfo;
using Tolarian.Copyshop.Business.Utility;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class DeckImportInteractor : IDeckImportInteractor
    {
        private readonly ICardDataGateway _cardGateway;
        private readonly ISetDataGateway _setGateway;

        private Dictionary<string, string> arenaSetCodesToScryfallCodesMap = new Dictionary<string, string>();

        public DeckImportInteractor(ICardDataGateway cardGateway, ISetDataGateway setGateway)
        {
            _cardGateway = cardGateway;
            _setGateway = setGateway;
        }

        public (List<SfCard> Cards, string NotFound) GetCardsForImport(List<string> importLines)
        {
            Dictionary<GetCardCollectionRequest, int> requestsToCopiesMap
                = CardRequestResolver.ResolveCardRequestsFromImportString(importLines);

            var requests = requestsToCopiesMap.Keys.ToList();

            TranslateSetCodesToScryfall(requests);

            SfCardCollection firstTryResponse = _cardGateway.GetCardCollectionByIdentifiers(requests);
            SfCard[] foundOnFirstTry = firstTryResponse.Data;
            SfIdentifier[] missedIdentifiers = firstTryResponse.NotFound;

            SfCardCollection secondTryResponse = _cardGateway.GetCardCollectionByIdentifiers(missedIdentifiers.Select(i => new GetCardCollectionRequest { Name = i.Name, SetCode = null }).ToList());

            List<SfCard> importedCards = foundOnFirstTry.Concat(secondTryResponse.Data).ToList();
            List<SfCard> importedDeck = AddCardsInCorrectAmount(requestsToCopiesMap, requests, importedCards);

            return (importedDeck, FormatNotFoundListIn(secondTryResponse));
        }
        List<SfCard> AddCardsInCorrectAmount(Dictionary<GetCardCollectionRequest, int> requestsToCopiesMap, List<GetCardCollectionRequest> requests, List<SfCard> importedCards)
        {
            List<SfCard> result = new List<SfCard>();

            foreach (var card in importedCards)
            {
                GetCardCollectionRequest formerRequest = FindOriginalRequestInList(card, requests);

                int amountOfCopies = requestsToCopiesMap[formerRequest];
                result.AddRange(Enumerable.Repeat(card, amountOfCopies));
            }

            return result;
        }

        private static GetCardCollectionRequest FindOriginalRequestInList(SfCard card, List<GetCardCollectionRequest> requests)
        {
            var formerRequest = requests.FirstOrDefault(r => string.Equals(r.Name, card.Name, StringComparison.InvariantCultureIgnoreCase));

            if (formerRequest == null)
            {
                //This will find the original request if the card at hand is a dual faced card which name is the sum of the two face's names separated by // e.g. [Dusk // Dawn]
                formerRequest = requests.FirstOrDefault(r => r.Name == card.Name.Split(new string[] { " // " }, StringSplitOptions.RemoveEmptyEntries)[0] || r.Name
                == card.Name.Split(new string[] { " // " }, StringSplitOptions.RemoveEmptyEntries)[1]); ;
            }

            return formerRequest;
        }

        private void TranslateSetCodesToScryfall(List<GetCardCollectionRequest> requests)
        {
            requests.ForEach(request => request.SetCode = TranslateSetCode(request.SetCode));
        }

        private string TranslateSetCode(string setCode)
        {
            if (setCode == null)
                return null;

            if(arenaSetCodesToScryfallCodesMap.ContainsKey(setCode.ToLower()))
            {
                return arenaSetCodesToScryfallCodesMap[setCode] ?? setCode;
            }
            else
            {
                SfSet correspondingSfSet = _setGateway.GetAllSets().Data.FirstOrDefault(set => string.Equals(set.MagicArenaSetCode, setCode, StringComparison.InvariantCultureIgnoreCase));

                string scryfallSetCode = correspondingSfSet != null ? correspondingSfSet.ScryfallSetCode : null;
                arenaSetCodesToScryfallCodesMap[setCode] = scryfallSetCode;

                return scryfallSetCode ?? setCode;
            }
        }

        private string FormatNotFoundListIn(SfCardCollection source)
        {
            var notFound = source.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode)).Distinct();

            return string.Join(Environment.NewLine, notFound);
        }

    }
}
