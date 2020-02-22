using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller
{
    public class DeckController
    {
        public int GetTotalCardsOfDeck(List<IFullCard> deckCards)
        {
            IEnumerable<(Guid Key, int Count, int CardCount)> groupedCards = deckCards.GroupBy(o => o.Id, (id, cards) => (
                Key: id,
                Count: cards.Count(),
                CardCount: cards.Max(card => card.CardCount)
            ));

            int totalCardCount = 0;
            foreach ((Guid Key, int Count, int CardCount) in groupedCards)
            {
                totalCardCount += CardCount;
            }
            return totalCardCount;
        }
    }
}
