using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
            this._cardGateway = cardGateway;
            this._setCodeTranslator = setCodeTranslator;
            this._importStringParser = importStringParser;
        }

        public (List<SfCard> Cards, string NotFound) GetCardsForImport(List<string> importLines)
        {
            List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap
                = this._importStringParser.ResolvePreImportCardsFromImportString(importLines);

            List<GetCardCollectionRequest> requests = cardsToCopiesMap.Select(pair => new GetCardCollectionRequest { Name = pair.Key.CardName, SetCode = pair.Key.SetCode }).ToList();

            this.TranslateSetCodesToScryfall(requests);

            SfCardCollection firstTryResponse = this._cardGateway.GetCardCollectionByIdentifiers(requests);
            List<SfCard> foundOnFirstTry = firstTryResponse.Data.ToList();
            SfIdentifier[] missedIdentifiers = firstTryResponse.NotFound;
            List<KeyValuePair<PreImportCard, int>> cardsToCopiesMapWithoutMissed = GetMapWithoutMissedCards(cardsToCopiesMap, missedIdentifiers);
            List<SfCard> importedDeck = this.AddCardsInCorrectAmount(cardsToCopiesMapWithoutMissed, foundOnFirstTry);
            cardsToCopiesMap.RemoveAll(x => cardsToCopiesMapWithoutMissed.Contains(x));

            SfCardCollection secondTryResponse = SfCardCollection.GetEmpty();

            if (missedIdentifiers.Any())
            {
                secondTryResponse = this._cardGateway.GetCardCollectionByIdentifiers(missedIdentifiers.Select(i => new GetCardCollectionRequest { Name = i.Name, SetCode = null }).ToList());
                importedDeck.AddRange(this.AddCardsInCorrectAmount(GetMapWithoutMissedCards(cardsToCopiesMap, secondTryResponse.NotFound), secondTryResponse.Data.ToList()));
            }

            return (importedDeck, this.FormatNotFoundListIn(secondTryResponse));
        }

        private static List<KeyValuePair<PreImportCard, int>> GetMapWithoutMissedCards(List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap, SfIdentifier[] missedIdentifiers)
        {
            return cardsToCopiesMap.Where(x => !missedIdentifiers.Select(mi => mi.Name).Contains(x.Key.CardName)).ToList();
        }

        private void TranslateSetCodesToScryfall(List<GetCardCollectionRequest> requests)
        {
            requests.ForEach(request => request.SetCode = this._setCodeTranslator.TranslateArenaCodeToScryfallCode(request.SetCode));
        }

        private List<SfCard> AddCardsInCorrectAmount(List<KeyValuePair<PreImportCard, int>> cardsToCopiesMap, List<SfCard> importedCards)
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

        private string FormatNotFoundListIn(SfCardCollection source)
        {
            IEnumerable<string> notFound = source.NotFound.Select(nf => string.Join(" ", nf.Name, nf.SetCode)).Distinct();

            return string.Join(Environment.NewLine, notFound);
        }

        public (List<SfCard>, string notFound) ImportFromTappedOut(Uri deckUrl)
        {
            HtmlDocument htmlDoc;
            using (WebClient client = new WebClient())
            {
                htmlDoc = new HtmlDocument();
                using (System.IO.Stream domStream = client.OpenRead(deckUrl))
                {
                    htmlDoc.Load(domStream);
                }
            }

            HtmlNode boardContainer = htmlDoc.DocumentNode.SelectSingleNode("descendant-or-self::div[@class='row board-container']");
            const string nameAttribute = "data-name";
            List<string> importLines = boardContainer.SelectNodes("descendant::li[@class='member']/a")
                                                               .Select(node => $"{node.InnerText.TrimEnd('x')} {HttpUtility.HtmlDecode(node.GetAttributeValue(nameAttribute, "Error"))}").ToList();

            (List<SfCard> Cards, string NotFound) result = this.GetCardsForImport(importLines);
            return result;
        }

    }
}
