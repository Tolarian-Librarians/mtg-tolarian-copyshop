using System;
using System.Linq;
using System.Collections.Generic;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.DeckInfo;

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
    }
}
