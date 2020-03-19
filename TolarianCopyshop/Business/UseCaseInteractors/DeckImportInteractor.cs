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
            //Scryfall will return a maximum of 75 Cards per request
            int scryfallApiReturnCountMaximum = 75;

            List<GetCardCollectionRequest> resolvedRequests = DeckImportHelper.ResolveCardRequestsFromImportString(importLines);

            TranslateSetCodesToScryfall(resolvedRequests);

            List<List<GetCardCollectionRequest>> requestLists = ChunkListBySize(resolvedRequests, scryfallApiReturnCountMaximum);

            IEnumerable<SfCardCollection> dbResponse = requestLists.Select(r => _cardGateway.GetCardCollectionByIdentifiers(r));

            var missedIdentifiers = dbResponse.SelectMany(r => r.NotFound);
            var retry = _cardGateway.GetCardCollectionByIdentifiers(missedIdentifiers.Select(i => new GetCardCollectionRequest { Name = i.Name, SetCode = null}).ToList());

            string notFound = FormatNotFoundListFrom(new List<SfCardCollection> { retry });

            dbResponse.Append(retry);

            List<SfCard> response = new List<SfCard>(retry.Data);

            return (dbResponse.SelectMany(cc => cc.Data).ToList(), notFound);
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

        private List<List<T>> ChunkListBySize<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        private string FormatNotFoundListFrom(IEnumerable<SfCardCollection> source)
        {
            var notFound = source.SelectMany(cc => cc.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode))).Distinct().ToArray();

            return string.Join(Environment.NewLine, notFound);
        }

    }
}
