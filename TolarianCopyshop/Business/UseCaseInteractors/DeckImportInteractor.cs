using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using HtmlAgilityPack;

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

            List<GetCardCollectionRequest> requests = cardsToCopiesMap.Select(pair => new GetCardCollectionRequest { Name = pair.Key.CardName, SetCode = pair.Key.SetCode }).ToList();

            TranslateSetCodesToScryfall(requests);

            SfCardCollection firstTryResponse = _cardGateway.GetCardCollectionByIdentifiers(requests);
            List<SfCard> foundOnFirstTry = firstTryResponse.Data.ToList();
            SfIdentifier[] missedIdentifiers = firstTryResponse.NotFound;
            List<KeyValuePair<PreImportCard, int>> cardsToCopiesMapWithoutMissed = GetMapWithoutMissedCards(cardsToCopiesMap, missedIdentifiers);
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

        private static List<SfCard> AddCardsInCorrectAmount(List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap, List<SfCard> importedCards)
        {
            List<SfCard> result = new();

            for (int i = 0; i < importedCards.Count; i++)
            {
                int amountOfCopies = cardsToCopiesMap[i].Value;
                SfCard card = importedCards[i];

                result.AddRange(Enumerable.Repeat(card, amountOfCopies));
            }

            return result;
        }

        private static string FormatNotFoundListIn(SfCardCollection source)
        {
            IEnumerable<string> notFound = source.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode)).Distinct();

            return string.Join(Environment.NewLine, notFound);
        }

        public (List<SfCard>, string notFound) ImportFromTappedOut(Uri deckUrl)
        {
            HtmlDocument htmlDoc;
            using (WebClient client = new())
            {
                htmlDoc = new HtmlDocument();
                using System.IO.Stream domStream = client.OpenRead(deckUrl);
                htmlDoc.Load(domStream);
            }

            HtmlNode boardContainer = htmlDoc.DocumentNode.SelectSingleNode("descendant-or-self::div[@class='row board-container']");
            List<string> importLines = boardContainer.SelectNodes("descendant::li[@class='member']/a")
                                                               .Select(node => GetAsImportString(node, false)).ToList();

            //Commander has a different place inside the dom than the regular cards
            HtmlNode commanderNode = boardContainer.SelectSingleNode("descendant::img[@class='commander-img']/parent::a");
            if (commanderNode != null)
            {
                importLines.Add(GetAsImportString(commanderNode, true));
            }

            (List<SfCard> Cards, string NotFound) result = GetCardsForImport(importLines);
            return result;
        }

        private static string GetAsImportString(HtmlNode node, bool isCommander)
        {
            const string nameAttribute = "data-name";
            return $"{(isCommander ? "1" : node.InnerText.TrimEnd('x'))} {HttpUtility.HtmlDecode(node.GetAttributeValue(nameAttribute, "Error"))}";
        }
    }
}