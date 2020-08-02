using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.DbRequestModels;
using Tolarian.Copyshop.Business.EntitiesModels;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class DeckImportInteractor : IDeckImportInteractor
    {
        private readonly ICardDataGateway _cardGateway;
        private readonly ISetCodeTranslator _setCodeTranslator;
        private readonly IImportStringParser _importStringParser;

        public DeckImportInteractor(ICardDataGateway cardGateway, ISetCodeTranslator setCodeTranslator, IImportStringParser importStringParser)
        {
            _cardGateway = cardGateway;
            _setCodeTranslator = setCodeTranslator;
            _importStringParser = importStringParser;
        }

        public (List<SfCard> Cards, string NotFound) GetCardsForImport(List<string> importLines)
        {
            List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap
                = _importStringParser.ResolvePreImportCardsFromImportString(importLines);

            var requests = cardsToCopiesMap.Select(pair => new GetCardCollectionRequest { Name = pair.Key.CardName, SetCode = pair.Key.SetCode }).ToList();

            TranslateSetCodesToScryfall(requests);

            SfCardCollection firstTryResponse = _cardGateway.GetCardCollectionByIdentifiers(requests);
            var foundOnFirstTry = firstTryResponse.Data.ToList();
            SfIdentifier[] missedIdentifiers = firstTryResponse.NotFound;
            var cardsToCopiesMapWithoutMissed = GetMapWithoutMissedCards(cardsToCopiesMap, missedIdentifiers);
            List<SfCard> importedDeck = AddCardsInCorrectAmount(cardsToCopiesMapWithoutMissed, foundOnFirstTry);
            cardsToCopiesMap.RemoveAll(x => cardsToCopiesMapWithoutMissed.Contains(x));

            SfCardCollection secondTryResponse = SfCardCollection.GetEmpty();

            if (missedIdentifiers.Any())
            {
                secondTryResponse = _cardGateway.GetCardCollectionByIdentifiers(missedIdentifiers.Select(i => new GetCardCollectionRequest { Name = i.Name, SetCode = null }).ToList());
                importedDeck.AddRange(AddCardsInCorrectAmount(GetMapWithoutMissedCards(cardsToCopiesMap, secondTryResponse.NotFound), secondTryResponse.Data.ToList()));
            }

            return (importedDeck, FormatNotFoundListIn(secondTryResponse));
        }

        private static List<KeyValuePair<PreImportCard, int>> GetMapWithoutMissedCards(List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap, SfIdentifier[] missedIdentifiers)
        {
            return cardsToCopiesMap.Where(x => !missedIdentifiers.Select(mi => mi.Name).Contains(x.Key.CardName)).ToList();
        }

        private void TranslateSetCodesToScryfall(List<GetCardCollectionRequest> requests)
        {
            requests.ForEach(request => request.SetCode = _setCodeTranslator.TranslateArenaCodeToScryfallCode(request.SetCode));
        }

        List<SfCard> AddCardsInCorrectAmount(List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap, List<SfCard> importedCards)
        {
            List<SfCard> result = new List<SfCard>();

            for (int i = 0; i < importedCards.Count; i++)
            {
                int amountOfCopies = cardsToCopiesMap[i].Value;
                SfCard card = importedCards[i];

                result.AddRange(Enumerable.Repeat(card, amountOfCopies));
            }

            return result;
        }

        private PreImportCard FindPreImportCard(SfCard card, List<PreImportCard> preImportCards)
        {
            const string cardNamesSeparator = " // ";

            string nameToFind = card.Name;
            PreImportCard result = preImportCards.FirstOrDefault(pre => string.Equals(pre.CardName, nameToFind, StringComparison.InvariantCultureIgnoreCase));

            if (result == null && card.Name.Contains(cardNamesSeparator))
            {
                string[] nameSplitted = card.Name.Split(new string[] { cardNamesSeparator }, StringSplitOptions.RemoveEmptyEntries);

                //This will find the original request if the card at hand is a multi faced card whichs name is the sum of the two face's names separated by // e.g. [Dusk // Dawn]
                result = preImportCards.FirstOrDefault(pre => pre.CardName == nameSplitted[0] || pre.CardName == nameSplitted[1]); ;
            }

            return result ?? throw new InvalidOperationException($"Could not find the preImportCard for a card named {card.Name}!");
        }

        private string FormatNotFoundListIn(SfCardCollection source)
        {
            var notFound = source.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode)).Distinct();

            return string.Join(Environment.NewLine, notFound);
        }

    }
}
