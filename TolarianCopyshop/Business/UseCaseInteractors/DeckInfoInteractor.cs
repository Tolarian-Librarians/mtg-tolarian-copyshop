using System;
using System.Collections.Generic;
using System.Linq;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Business.Models.Enums;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class DeckInfoInteractor : IDeckInfoInteractor
    {
        public int GetTotalCardCountOfDeck(List<DeckInfoCard> deck)
        {
            int cardCountInDeck = 0;
            var playables = GetOnlyPlayables(deck, true);
            playables.ForEach(card => cardCountInDeck += card.Copies);
            return cardCountInDeck;
        }

        public Dictionary<CardType, int> GetCardTypeCounts(List<DeckInfoCard> deck)
        {
            var playables = GetOnlyPlayables(deck, true);

            var result = new Dictionary<CardType, int>();

            foreach (var cardType in Enum.GetValues(typeof(CardType)).Cast<CardType>())
            {
                result.Add(cardType, playables.Sum(c => c.CardFaces[0].PrimaryCardType == cardType ? c.Copies : 0));
            }

            return result;
        }

        public Dictionary<MtgColor, int> GetManaSourcesCounts(List<DeckInfoCard> deck)
        {
            var result = new Dictionary<MtgColor, int>();

            var playables = GetOnlyPlayables(deck, true);

            foreach (var color in Enum.GetValues(typeof(MtgColor)).Cast<MtgColor>())
            {
                result.Add(color, playables.Sum(c => c.ProducedMana?.Contains(color) ?? false ? c.Copies : 0));
            }

            return result;
        }

        public Dictionary<MtgColor, int> GetColorSymbolCounts(List<DeckInfoCard> deck)
        {
            var result = new Dictionary<MtgColor, int>();

            var playables = GetOnlyPlayables(deck, false);

            foreach (var color in Enum.GetValues(typeof(MtgColor)).Cast<MtgColor>())
            {
                result.Add(color, playables.Select(c => (c.ManaCostLine?.Count(cha => cha == Convert.ToChar(color.ToString())) ?? default) * c.Copies).Sum());
            }

            return result;
        }

        public Dictionary<MtgColor, int> GetColorCardCounts(List<DeckInfoCard> deck)
        {
            var result = new Dictionary<MtgColor, int>();

            foreach (MtgColor color in Enum.GetValues(typeof(MtgColor)).Cast<MtgColor>())
            {
                result.Add(color, 0);
            }

            var playables = GetOnlyPlayables(deck, false);

            foreach (var card in playables)
            {
                if (card.Colors is null)
                {
                    if (card.CardFaces is null || card.CardFaces[0].Colors is null)
                    {
                        result[MtgColor.C] += card.Copies;
                    }
                    else
                    {
                        result[GetColorFromCardData(card.CardFaces[0].Colors)] += card.Copies;
                    }
                }
                else
                {
                    result[GetColorFromCardData(card.Colors)] += card.Copies;
                }
            }

            return result;
        }

        private MtgColor GetColorFromCardData(List<MtgColor> color)
        {
            if (color is null || color.Count == 0)
            {
                return MtgColor.C;
            }
            else if (color.Count > 1)
            {
                return MtgColor.M;
            }
            else
            {
                return color[0];
            }
        }

        public Dictionary<float, int> GetCreatureManaCurve(List<DeckInfoCard> deck)
        {
            var creatures = GetOnlyPlayables(deck, false).Where(c => c.CardFaces[0].PrimaryCardType == CardType.Creature);
            return GetManaCurve(creatures);
        }

        public Dictionary<float, int> GetNonCreatureManaCurve(List<DeckInfoCard> deck)
        {
            var nonCreatures = GetOnlyPlayables(deck, false).Where(c => c.CardFaces[0].PrimaryCardType != CardType.Creature);
            return GetManaCurve(nonCreatures);
        }

        private Dictionary<float, int> GetManaCurve(IEnumerable<DeckInfoCard> deck)
        {
            var grouped = deck.GroupBy(c => c.ConvertedManaCost, c => c.ConvertedManaCost, (cmc, countOfCards) => new
            {
                Cmc = cmc,
                CountOfCards = countOfCards.Count()
            }).OrderBy(x => x.Cmc);

            var result = grouped.ToDictionary(gr => gr.Cmc, gr => gr.CountOfCards);
            return result;
        }

        public float GetAverageCmc(List<DeckInfoCard> deck)
        {
            var playables = GetOnlyPlayables(deck, false);
            var totalCards = GetTotalCardCountOfDeck(playables);
            var summedUpCmcs = playables.Sum(c => c.ConvertedManaCost * c.Copies);

            if (totalCards == 0)
            {
                return 0;
            }
            else
            {
                return (float)Math.Round(summedUpCmcs / totalCards, 2);
            }
        }

        public int GetCreatureCount(List<DeckInfoCard> deck)
        {
            var playables = GetOnlyPlayables(deck, false);

            return playables.Sum(c => c.CardFaces[0].PrimaryCardType == CardType.Creature ? c.Copies : 0);
        }

        public int GetNonCreatureCount(List<DeckInfoCard> deck)
        {
            var playables = GetOnlyPlayables(deck, false);

            return playables.Sum(c => c.CardFaces[0].PrimaryCardType != CardType.Creature ? c.Copies : 0);
        }

        private List<DeckInfoCard> GetOnlyPlayables(List<DeckInfoCard> deck, bool includeLands)
        {
            return deck.Where(dc => dc.CardFaces.Any(cf => cf.PrimaryCardType != CardType.Token &&
                                                    cf.PrimaryCardType != CardType.Emblem &&
                                                    cf.PrimaryCardType != CardType.Unknown &&
                                                    (includeLands || cf.PrimaryCardType != CardType.Land)
            )).ToList();
        }
    }
}