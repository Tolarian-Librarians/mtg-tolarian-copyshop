using System.Collections.Generic;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Business.Models.Enums;
using System.Linq;

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
            var playables = GetOnlyPlayables(deck, true);
            return new Dictionary<CardType, int>();
        }

        public Dictionary<MtgColor, int> GetManaSourcesCounts(List<DeckInfoCard> deck)
        {
            var result = new Dictionary<MtgColor, int>
            {
                {MtgColor.B, 0},
                {MtgColor.G, 0},
                {MtgColor.R, 0},
                {MtgColor.U, 0},
                {MtgColor.W, 0},
            };

            foreach (var card in GetOnlyPlayables(deck, true))
            {
                if (card.ProducedMana == null)
                    continue;

                if (card.ProducedMana.Contains(MtgColor.B))
                    result[MtgColor.B]++;                
                if (card.ProducedMana.Contains(MtgColor.U))
                    result[MtgColor.U]++;                
                if (card.ProducedMana.Contains(MtgColor.R))
                    result[MtgColor.R]++;               
                if (card.ProducedMana.Contains(MtgColor.G))
                    result[MtgColor.G]++;               
                if (card.ProducedMana.Contains(MtgColor.W))
                    result[MtgColor.W]++;
            }

            return result;
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

            foreach (var card in GetOnlyPlayables(deck, false))
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
            var playables = GetOnlyPlayables(deck, false);
            var grouped = playables.GroupBy(c => c.ConvertedManaCost, c => c.ConvertedManaCost, (cmc, countOfCards) => new
            {
                Cmc = cmc,
                CountOfCards = countOfCards.Count()
            }).OrderBy(x => x.Cmc);

            var result = grouped.ToDictionary(gr => gr.Cmc, gr => gr.CountOfCards);
            return result;
        }

        private List<DeckInfoCard> GetOnlyPlayables(List<DeckInfoCard> deck, bool includeLands)
        {
            return deck.Where(dc => dc.cardFaces.Any(cf => cf.CardType != CardType.Token && 
                                                    cf.CardType != CardType.Emblem && 
                                                    cf.CardType != CardType.Unknown && 
                                                    (includeLands || cf.CardType != CardType.Land)
            )).ToList();
        }
    }
}
