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
            Dictionary<PreImportCard, int> cardsToCopiesMap
                = _importStringParser.ResolvePreImportCardsFromImportString(importLines);

            var requests = cardsToCopiesMap.Keys.Select(key => new GetCardCollectionRequest { Name = key.CardName, SetCode = key.SetCode }).ToList();

            TranslateSetCodesToScryfall(requests);

            SfCardCollection firstTryResponse = _cardGateway.GetCardCollectionByIdentifiers(requests);
            SfCard[] foundOnFirstTry = firstTryResponse.Data;
            SfIdentifier[] missedIdentifiers = firstTryResponse.NotFound;
            SfCardCollection secondTryResponse = SfCardCollection.GetEmpty();

            if (missedIdentifiers.Any())
                secondTryResponse = _cardGateway.GetCardCollectionByIdentifiers(missedIdentifiers.Select(i => new GetCardCollectionRequest { Name = i.Name, SetCode = null }).ToList());

            List<SfCard> importedCards = foundOnFirstTry.Concat(secondTryResponse.Data).ToList();
            List<SfCard> importedDeck = AddCardsInCorrectAmount(cardsToCopiesMap, importedCards);

            return (importedDeck, FormatNotFoundListIn(secondTryResponse));
        }

        private void TranslateSetCodesToScryfall(List<GetCardCollectionRequest> requests)
        {
            requests.ForEach(request => request.SetCode = _setCodeTranslator.TranslateArenaCodeToScryfallCode(request.SetCode));
        }

        List<SfCard> AddCardsInCorrectAmount(Dictionary<PreImportCard, int> cardsToCopiesMap, List<SfCard> importedCards)
        {
            List<SfCard> result = new List<SfCard>();

            foreach (var card in importedCards)
            {
                PreImportCard preImportCard = FindPreImportCard(card, cardsToCopiesMap.Keys.ToList());

                int amountOfCopies = cardsToCopiesMap[preImportCard];
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
