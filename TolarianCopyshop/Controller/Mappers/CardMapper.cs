using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Models.Enums;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.Mappers
{
    internal abstract class CardMapper
    {
        internal static List<Guid> GetTokenGuidsOfDeck(List<IFullCard> deck)
        {
            IEnumerable<IFullCard> nontokenCards = deck.Where(dc => !dc.CardFaces.Any(cf => cf.PrimaryCardType == ResponseObjects.Enums.CardType.Token));

            List<Guid> tokenGuids = nontokenCards.SelectMany(dc => dc?.RelatedCards?.Where(rc => rc?.Type == RelatedCardType.token)?.Select(rc => rc.Id)
            ?? new List<Guid>())?.ToList();

            return tokenGuids;
        }

        internal static List<string> PrepareImportStringForBusiness(string source)
        {
            List<string> result = source.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None).ToList();

            return result;
        }

        internal static List<ArtworkCard> MapToArtworkDto(List<SfCard> source)
        {
            List<ArtworkCard> result = source.Select(card =>
            {
                return new ArtworkCard
                {
                    SetCode = card.SetCode.ToUpper(),
                    SetName = TruncateSetname(card.SetName),
                    PrintId = card.PrintId,
                    Image = card.IsTransformable ? card.CardFaces[0].ImageUris[CardImageTypes.Small] : card.ImageUris[CardImageTypes.Small],
                    ReleaseDate = card.ReleaseDate,
                };
            }).ToList();

            return result;
        }

        private static string TruncateSetname(string setName)
        {
            const int maxLength = 20;
            if (setName.Length <= maxLength)
            {
                return setName;
            }

            return string.Concat(setName.Substring(0, maxLength), "...");
        }

        internal static SearchCard MapToSearchResultDto(SfCard source)
        {
            SearchCard result = new SearchCard
            {
                Name = source.Name,
                PrimaryCardType = GetPrimaryCardType(GetBaseCardTypesFromTypeLine(source.TypeLine)),
                PrintId = source.PrintId,
                Image = source.IsTransformable ? source.CardFaces[0].ImageUris[CardImageTypes.Normal] : source.ImageUris[CardImageTypes.Normal],
                PowerToughness = string.IsNullOrWhiteSpace(source.Power) && string.IsNullOrWhiteSpace(source.Toughness) ? "" : $"{source.Power,-2}/{source.Toughness,2}",
            };

            return result;
        }

        internal static CardSearchResponse MapToSearchResultDto(List<SfCard> source, string resultsCount)
        {
            List<SearchCard> foundCards = source.Select(card => MapToSearchResultDto(card)).ToList();

            return new CardSearchResponse
            {
                ResultsCount = resultsCount,
                Results = foundCards
            };
        }

        internal static List<IFullCard> MapToCardDto(List<SfCard> sources)
        {
            List<IFullCard> result = sources.Select(card => MapToCardDto(card)).ToList();
            return result;
        }

        internal static IFullCard MapToCardDto(SfCard source)
        {
            FullCard result = new FullCard
            {
                CardId = source.CardId,
                PrintId = source.PrintId,
                FormattedCardName = GetFormattedNameOfCard(source),
                Legalities = source.Legalities.ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' ')),
                CardCount = 1,
                CardFaces = MapCardFacesOf(source),
                RelatedCards = MapRelatedCardsOf(source),
                IsTransformable = source.IsTransformable,
                ConvertedManaCost = source.ConvertedManaCost,
                ColorIdentity = source.ColorIdentity?.Select(c => (ResponseObjects.Enums.MtgColor)((int)c)).ToList(),
                Colors = source.Colors?.Select(c => (ResponseObjects.Enums.MtgColor)((int)c)).ToList(),
                ManaCostLine = source.ManaCostLine ?? source.CardFaces?.FirstOrDefault()?.ManaCostLine,
                ProducedMana = source.ProducedMana?.Select(c => (ResponseObjects.Enums.MtgColor)((int)c)).ToList(),
                SetCode = source.SetCode,
            };

            return result;
        }

        private static ICollection<CardFace> MapCardFacesOf(SfCard source)
        {
            List<CardFace> result = new List<CardFace>();
            if (source.IsTransformable)
            {
                result = source.CardFaces.Select(cf =>
                {
                    List<ResponseObjects.Enums.CardType> typesOfCard = GetBaseCardTypesFromTypeLine(cf.TypeLine);

                    return new CardFace
                    {
                        LargeImage = cf.ImageUris.ContainsKey(CardImageTypes.Large) ? cf.ImageUris[CardImageTypes.Large] : null,
                        SmallImage = cf.ImageUris.ContainsKey(CardImageTypes.Small) ? cf.ImageUris[CardImageTypes.Small] : null,
                        CroppedImage = cf.ImageUris.ContainsKey(CardImageTypes.Border_Crop) ? cf.ImageUris[CardImageTypes.Border_Crop] : null,
                        Colors = cf.Colors?.Select(c => (ResponseObjects.Enums.MtgColor)((int)c)).ToList(),
                        Name = cf.Name,
                        Text = cf.Text,
                        CardTypes = typesOfCard,
                        PrimaryCardType = GetPrimaryCardType(typesOfCard),
                    };
                }
                ).ToList();
            }
            else
            {
                List<ResponseObjects.Enums.CardType> typesOfCard = GetBaseCardTypesFromTypeLine(source.TypeLine);
                result.Add(new CardFace
                {
                    LargeImage = source.ImageUris.ContainsKey(CardImageTypes.Large) ? source.ImageUris[CardImageTypes.Large] : null,
                    SmallImage = source.ImageUris.ContainsKey(CardImageTypes.Small) ? source.ImageUris[CardImageTypes.Small] : null,
                    CroppedImage = source.ImageUris.ContainsKey(CardImageTypes.Border_Crop) ? source.ImageUris[CardImageTypes.Border_Crop] : null,
                    Colors = source.Colors?.Select(c => (ResponseObjects.Enums.MtgColor)((int)c)).ToList(),
                    Name = source.Name,
                    Text = GetTextOfCard(source),
                    CardTypes = typesOfCard,
                    PrimaryCardType = GetPrimaryCardType(typesOfCard),
                });
            }

            return result;
        }

        private static ICollection<RelatedCard> MapRelatedCardsOf(SfCard source)
        {
            List<RelatedCard> result = new List<RelatedCard>();

            if (source.RelatedCards != null)
            {
                result = source.RelatedCards.Select(rc => new RelatedCard
                {
                    Id = rc.Id,
                    Type = (RelatedCardType)(int)rc.Type
                }).ToList();
            }

            return result;
        }

        private static string GetFormattedNameOfCard(SfCard source)
        {
            if (source.IsTransformable)
            {
                return string.Join(" // ", source.CardFaces.Select(c => c.Name).ToArray());
            }
            else
            {
                return source.Name ?? "";
            }
        }

        private static string GetTextOfCard(SfCard source)
        {
            if (source.IsMultifaced)
            {
                return string.Join(" // ", source.CardFaces.Select(c => c.Text).ToArray());
            }
            else
            {
                return source.Text ?? "";
            }
        }

        private static List<ResponseObjects.Enums.CardType> GetBaseCardTypesFromTypeLine(string cardType)
        {
            List<string> typesOfCard = cardType.Split(' ').ToList();

            typesOfCard = typesOfCard.Select(str => str.ToLower().Trim()).ToList();
            typesOfCard.RemoveAll(str => string.IsNullOrWhiteSpace(str) || str == "-" || str == "—");

            List<ResponseObjects.Enums.CardType> result = new List<ResponseObjects.Enums.CardType>();

            foreach (string type in typesOfCard)
            {
                if (Enum.TryParse(type, true, out ResponseObjects.Enums.CardType parsed))
                {
                    result.Add(parsed);
                }
            }

            if (!result.Any())
            {
                result.Add(ResponseObjects.Enums.CardType.Unknown);
            }

            return result;

        }

        private static ResponseObjects.Enums.CardType GetPrimaryCardType(List<ResponseObjects.Enums.CardType> cardTypes)
        {
            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Token))
            {
                return ResponseObjects.Enums.CardType.Token;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Emblem))
            {
                return ResponseObjects.Enums.CardType.Emblem;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Creature))
            {
                return ResponseObjects.Enums.CardType.Creature;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Enchantment))
            {
                return ResponseObjects.Enums.CardType.Enchantment;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Sorcery))
            {
                return ResponseObjects.Enums.CardType.Sorcery;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Instant))
            {
                return ResponseObjects.Enums.CardType.Instant;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Land))
            {
                return ResponseObjects.Enums.CardType.Land;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Artifact))
            {
                return ResponseObjects.Enums.CardType.Artifact;
            }

            if (cardTypes.Contains(ResponseObjects.Enums.CardType.Planeswalker))
            {
                return ResponseObjects.Enums.CardType.Planeswalker;
            }

            return ResponseObjects.Enums.CardType.Unknown;
        }
    }
}
