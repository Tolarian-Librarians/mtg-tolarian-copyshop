using AutoMapper;
using Tolarian.Copyshop.Controller.ResponseObjects;
using System.Linq;
using Tolarian.Copyshop.Business.Models.Enums;
using System.Collections;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.Interfaces;
using System;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Business.Models.DeckInfo;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.ScreenPresenter.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<List<IFullCard>, List<DeckInfoCard>>().ConvertUsing(i => GetBusinessCardsFromApiCards(i));
        }

        private CardType GetBaseCardTypeFromTypeLine(string cardType)
        {
            List<string> typesOfCard = cardType.Split(' ').ToList();

            typesOfCard = typesOfCard.Select(str => str.ToLower().Trim()).ToList();
            typesOfCard.RemoveAll(str => string.IsNullOrWhiteSpace(str) || str == "-");

            if (typesOfCard.Contains("creature"))
                return CardType.Creature;

            if (typesOfCard.Contains("enchantment"))
                return CardType.Enchantment;

            if (typesOfCard.Contains("sorcery"))
                return CardType.Sorcery;
            
            if (typesOfCard.Contains("instant"))
                return CardType.Instant;
            
            if (typesOfCard.Contains("land"))
                return CardType.Land;
            
            if (typesOfCard.Contains("artifact"))
                return CardType.Artifact;
            
            if (typesOfCard.Contains("planeswalker"))
                return CardType.Planeswalker;                       

            return CardType.Unknown;
        }

        private List<DeckInfoCard> GetBusinessCardsFromApiCards(List<IFullCard> apiCards)
        {
            var result = new List<DeckInfoCard>();
            var alreadyMapped = new List<IFullCard>();

            foreach (IFullCard fullCard in apiCards)
            {
                if (alreadyMapped.Contains(fullCard))
                    continue;

                DeckInfoCard mappedCard = new DeckInfoCard
                {
                    Id = fullCard.Id,
                    CardType = "",
                    Copies = fullCard.CardCount,
                    cardFaces = new List<DeckInfoCardFace>
                    {
                        new DeckInfoCardFace{ CardType = fullCard.CardType.ToString() },
                    }
                };

                IFullCard otherFace = apiCards.FirstOrDefault(card => card.Id == fullCard.Id && card.Name != fullCard.Name && card != fullCard);
                if(otherFace != null)
                {
                    mappedCard.cardFaces.Add(new DeckInfoCardFace { CardType = otherFace.CardType.ToString() });

                    //Remember the other face so it wont be mapped as well
                    alreadyMapped.Add(otherFace);
                }

                result.Add(mappedCard);
            }
            return result;
        }

        private Dictionary<string, string> GetFirstHalfOfLegalities(SfCard source)
        {
            return source.Legalities.Take(GetHalfIndexOfDictionary(source.Legalities)).ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' '));
        }

        private Dictionary<string, string> GetSecondHalfOfLegalities(SfCard source)
        {
            return source.Legalities.Skip(GetHalfIndexOfDictionary(source.Legalities)).Take(source.Legalities.Count - GetHalfIndexOfDictionary(source.Legalities)).ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' '));
        }

        private string GetTextOfCard(SfCard source)
        {
            if(string.IsNullOrEmpty(source.Text) && source.CardFaces != null)
            {
                return string.Join(" // ", source.CardFaces.Select(c => c.Text).ToArray());
            }
            else
            {
                return source.Text ?? "";
            }
        }

        private int GetHalfIndexOfDictionary(ICollection collection)
        {
            int count = collection.Count;
            if (count % 2 == 0)
                return count / 2;
            else
                return count / 2 + 1;
        }
    }
}