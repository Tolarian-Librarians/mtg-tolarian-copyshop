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
            var alreadyMapped = new List<IFullCard>();

            foreach (IFullCard fullCard in sources)
            {
                if (alreadyMapped.Contains(fullCard))
                    continue;

                DeckInfoCard mappedCard = GetBusinessFrom(fullCard);

                IFullCard otherFace = GetOtherFace(fullCard);
                if (otherFace != null)
                {
                    mappedCard.cardFaces.Add(new DeckInfoCardFace { CardType = otherFace.CardType.ToString() });

                    //Remember the other face so it won't be mapped as well
                    alreadyMapped.Add(otherFace);
                }

                result.Add(mappedCard);
            }
            return result;

            DeckInfoCard GetBusinessFrom(IFullCard fullCard)
            {
                return new DeckInfoCard
                {
                    Id = fullCard.Id,
                    Copies = fullCard.CardCount,
                    cardFaces = new List<DeckInfoCardFace>
                    {
                        new DeckInfoCardFace{ CardType = fullCard.CardType.ToString() },
                    }
                };
            }

            IFullCard GetOtherFace(IFullCard firstFace)
            {
                return sources.FirstOrDefault(card => card.Id == firstFace.Id && card.Name != firstFace.Name && card != firstFace);
            }
        }
    }
}
