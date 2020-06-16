using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Controller.Interfaces;

namespace Tolarian.Copyshop.Controller.Mappers
{
    public abstract class DeckMapper
    {
        public static List<DeckInfoCard> MapDeckDtoToBusiness(List<IFullCard> sources)
        {
            var result = new List<DeckInfoCard>();
            foreach (IFullCard fullCard in sources)
            {
                DeckInfoCard mappedCard = GetBusinessFrom(fullCard);
                result.Add(mappedCard);
            }
            return result;

            DeckInfoCard GetBusinessFrom(IFullCard fullCard)
            {
                return new DeckInfoCard
                {
                    PrintId = fullCard.PrintId,
                    Copies = fullCard.CardCount,
                    cardFaces = fullCard.CardFaces.Select(cf => new DeckInfoCardFace 
                    {
                        CardType = cf.CardType.ToString(),
                    }).ToList(),
                };
            }
        }
    }
}
