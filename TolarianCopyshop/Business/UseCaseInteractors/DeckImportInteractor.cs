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
            List<GetCardCollectionRequest> resolvedRequests = DeckImportHelper.ResolveCardRequestsFromImportString(importLines);

            TranslateSetCodesToScryfall(resolvedRequests);

            SfCardCollection firstTryResponse = _cardGateway.GetCardCollectionByIdentifiers(resolvedRequests);

            SfCard[] foundOnFirstTry = firstTryResponse.Data;
            SfIdentifier[] missedIdentifiers = firstTryResponse.NotFound;
            SfCardCollection secondTryResponse = _cardGateway.GetCardCollectionByIdentifiers(missedIdentifiers.Select(i => new GetCardCollectionRequest { Name = i.Name, SetCode = null}).ToList());

            string notFound = FormatNotFoundListFrom(secondTryResponse );

            List<SfCard> importedCards = foundOnFirstTry.Concat(secondTryResponse.Data).ToList();

            return (importedCards, notFound);
        }

        private void TranslateSetCodesToScryfall(List<GetCardCollectionRequest> requests)
        {
            requests.ForEach(request => request.SetCode = TranslateSetCode(request.SetCode));
        }

        private string TranslateSetCode(string setCode)
        {
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

        private string FormatNotFoundListFrom(SfCardCollection source)
        {
            var notFound = source.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode)).Distinct();

            return string.Join(Environment.NewLine, notFound);
        }

    }
}
