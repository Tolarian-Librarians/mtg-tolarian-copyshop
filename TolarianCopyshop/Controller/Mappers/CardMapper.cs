using System;
using System.Collections.Generic;
using System.Linq;

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
                    Image = card.IsTransformable ? card.CardFaces[0].ImageUris.Thumb : card.ImageUris.Thumb,
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
            SearchCard result = new()
            {
                Name = source.Name,
                PrimaryCardType = GetPrimaryCardType(GetBaseCardTypesFromTypeLine(source.TypeLine)),
                PrintId = source.PrintId,
                Image = source.IsTransformable ? source.CardFaces[0].ImageUris.Grid : source.ImageUris.Grid,
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
            FullCard result = new()
            {
                CardId = GetOracleIdOf(source),
                PrintId = source.PrintId,
                FormattedCardName = GetFormattedNameOfCard(source),
                Legalities = source.Legalities.ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' ')),
                CardCount = 1,
                CardFaces = MapCardFacesOf(source),
                RelatedCards = MapRelatedCardsOf(source),
                IsTransformable = source.IsTransformable,
                ConvertedManaCost = source.ConvertedManaCost,
                ColorIdentity = MapColors(source.ColorIdentity),
                Colors = MapColors(source.Colors),
                ManaCostLine = source.ManaCostLine ?? source.CardFaces?.FirstOrDefault()?.ManaCostLine,
                ProducedMana = MapColors(source.ProducedMana),
                SetCode = source.SetCode,
            };

            return result;
        }

        private static ICollection<CardFace> MapCardFacesOf(SfCard source)
        {
            List<CardFace> result = new();
            if (source.IsTransformable)
            {
                result = source.CardFaces.Select(cf =>
                {
                    List<ResponseObjects.Enums.CardType> typesOfCard = GetBaseCardTypesFromTypeLine(cf.TypeLine);

                    return new CardFace
                    {
                        LargeImage = cf.ImageUris.Display,
                        SmallImage = cf.ImageUris.Thumb,
                        CroppedImage = cf.ImageUris.Crop,
                        Colors = MapColors(cf.Colors),
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
                    LargeImage = source.ImageUris.Display,
                    SmallImage = source.ImageUris.Thumb,
                    CroppedImage = source.ImageUris.Crop,
                    Colors = MapColors(source.Colors),
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
            List<RelatedCard> result = new();

            if (source.RelatedCards != null)
            {
                foreach (SfRelatedCard relatedCard in source.RelatedCards)
                {
                    // Components Scryfall introduces after this was written are irrelevant for us, so they are skipped
                    if (TryParseScryfallValue(relatedCard.Type, out RelatedCardType type))
                    {
                        result.Add(new RelatedCard { Id = relatedCard.Id, Type = type });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Reversible cards have no oracle id themselves, only their faces have one.
        /// </summary>
        private static Guid GetOracleIdOf(SfCard source)
            => source.CardId ?? source.CardFaces?.Select(cf => cf.CardId).FirstOrDefault(id => id.HasValue) ?? Guid.Empty;

        /// <summary>
        /// Maps Scryfall color symbols to <see cref="ResponseObjects.Enums.MtgColor"/>. Symbols unknown to the app are skipped.
        /// Keeps null as null, because some statistics treat a missing color list differently from an empty one.
        /// </summary>
        private static List<ResponseObjects.Enums.MtgColor> MapColors(IEnumerable<string> scryfallColors)
        {
            if (scryfallColors is null)
            {
                return null;
            }

            List<ResponseObjects.Enums.MtgColor> result = new();
            foreach (string scryfallColor in scryfallColors)
            {
                if (TryParseScryfallValue(scryfallColor, out ResponseObjects.Enums.MtgColor color))
                {
                    result.Add(color);
                }
            }

            return result;
        }

        /// <summary>
        /// Only accepts exact member names. <see cref="Enum.TryParse{TEnum}(string, out TEnum)"/> alone would also accept numbers and comma separated lists.
        /// </summary>
        private static bool TryParseScryfallValue<TEnum>(string value, out TEnum result) where TEnum : struct, Enum
        {
            result = default;
            return value != null
                && Enum.GetNames<TEnum>().Contains(value, StringComparer.OrdinalIgnoreCase)
                && Enum.TryParse(value, true, out result);
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

            List<ResponseObjects.Enums.CardType> result = new();

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