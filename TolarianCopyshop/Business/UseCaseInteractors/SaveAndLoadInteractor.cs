using System;
using System.Collections.Generic;
using System.IO;

using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.Models.SaveAndLoad;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class SaveAndLoadInteractor : ISaveAndLoadInteractor
    {
        private readonly ICardDataRequester _cardDataRequester;

        public SaveAndLoadInteractor(ICardDataRequester cardDataRequester)
        {
            _cardDataRequester = cardDataRequester;
        }

        public List<SfCard> LoadDeck(string fileName)
        {
            string line;
            List<Guid> requests = new();

            using StreamReader reader = new(fileName);

            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split('|');

                int cardCount = int.Parse(split[1]);

                for (int i = 0; i < cardCount; i++)
                {
                    requests.Add(Guid.Parse(split[0]));
                }
            }

            var result = _cardDataRequester.GetCardsByIds(requests);
            return result;
        }

        public void SaveDeck(List<SaveCard> cardsToSave, string fileName)
        {
            using StreamWriter writer = new(fileName);

            foreach (SaveCard card in cardsToSave)
            {
                writer.WriteLine($"{card.PrintId}|{card.CardCount}");
            }
        }
    }
}