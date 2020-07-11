using System.Collections.Generic;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Business.Models.Enums;
using System.Linq;
using System;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class DeckInfoInteractor : IDeckInfoInteractor
    {
        public int GetTotalCardCountOfDeck(List<DeckInfoCard> deck)
        {
            int cardCountInDeck = 0;
            deck.ForEach(card => cardCountInDeck += card.Copies);
            return cardCountInDeck;
        }

        public Dictionary<CardType, int> GetCardTypeCounts(List<DeckInfoCard> deck)
        {
            return new Dictionary<CardType, int>();
        }

        public Dictionary<MtgColor, int> GetColorSymbolCounts(List<DeckInfoCard> deck)
        {
            var result = new Dictionary<MtgColor, int>
            {
                {MtgColor.B, 0},
                {MtgColor.G, 0},
                {MtgColor.R, 0},
                {MtgColor.U, 0},
                {MtgColor.W, 0},
            };

            foreach (var card in deck)
            {
                result[MtgColor.B] += card.ManaCostLine.Count(c => c == 'B');
                result[MtgColor.G] += card.ManaCostLine.Count(c => c == 'G');
                result[MtgColor.R] += card.ManaCostLine.Count(c => c == 'R');
                result[MtgColor.U] += card.ManaCostLine.Count(c => c == 'U');
                result[MtgColor.W] += card.ManaCostLine.Count(c => c == 'W');
            }

            return result;
        }

        public Dictionary<float, int> GetManaCurve(List<DeckInfoCard> deck)
        {
            var grouped = deck.GroupBy(c => c.ConvertedManaCost, c => c.ConvertedManaCost, (cmc, countOfCards) => new
            {
                Cmc = cmc,
                CountOfCards = countOfCards.Count()
            });

            var result = grouped.ToDictionary(gr => gr.Cmc, gr => gr.CountOfCards);
            return result;
        }
    }
}
