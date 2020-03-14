using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Business.Models.Enums;
using Tolarian.Copyshop.Business.Models.SfCardInfo;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.Mappers
{
    public abstract class CardMapper
    {
        internal static List<CardArtworkResponse> MapToArtworkDto(List<SfCard> source)
        {
            List<CardArtworkResponse> result = source.Select(card =>
            {
                var convertedCard = new CardArtworkResponse
                {
                    SetCode = card.SetCode.ToUpper(),
                    SetName = TruncateSetname(card.SetName),
                    PrintId = card.PrintId,
                };
                if (card.IsTransformable)
                    convertedCard.Image = card.CardFaces[0].ImageUris[CardImageTypes.Border_Crop];
                else
                    convertedCard.Image = card.ImageUris[CardImageTypes.Border_Crop];

                return convertedCard;
            }
            ).ToList();

            return result;
        }

        private static string TruncateSetname(string setName)
        {
            const int maxLength = 20;
            if(setName.Length <= maxLength)
            {
                return setName;
            }

            return string.Concat(setName.Substring(0, maxLength), "...");
        }

        public static CardSearchCard MapToSearchResultDto(SfCard source)
        {
            var result = new CardSearchCard
            {
                Name = source.Name,
                CardType = GetBaseCardTypeFromTypeLine(source.TypeLine),
                PrintId = source.PrintId,
                Image = source.ImageUris?[CardImageTypes.Normal] ?? source.CardFaces?[0]?.ImageUris?[CardImageTypes.Normal],
            };

            return result;
        }

        public static CardSearchResponse MapToSearchResultDto(List<SfCard> source, string resultsCount)
        {
            List<CardSearchCard> foundCards = source.Select(card => MapToSearchResultDto(card)).ToList();

            return new CardSearchResponse
            {
                ResultsCount = resultsCount,
                Results = foundCards
            };
        }

        public static List<IFullCard> MapToCardDto(List<SfCard> sources)
        {
            var result =
                sources.SelectMany(card => MapToCardDto(card)).ToList();

            return result;
        }

        public static List<IFullCard> MapToCardDto(SfCard source)
        {
            var result = new List<FullCardResponse>();

            if(source.IsTransformable)
            {
                result.AddRange(MapDoubleSidedCardToDto(source));
            }
            else
            {
                result.Add(MapSingleFacedCardToDto(source));
            }

            return result.Cast<IFullCard>().ToList();
        }

        private static List<FullCardResponse> MapDoubleSidedCardToDto(SfCard source)
        {
            List<FullCardResponse> result = source.CardFaces.Select(card => new FullCardResponse
            {
                Name = card.Name,
                CardId = source.CardId,
                PrintId = source.PrintId,
                Text = card.Text,
                CardType = GetBaseCardTypeFromTypeLine(card.TypeLine),
                CardCount = 1,
                LargeImage = card.ImageUris.ContainsKey(CardImageTypes.Large) ? card.ImageUris[CardImageTypes.Large] : null,
                SmallImage = card.ImageUris.ContainsKey(CardImageTypes.Small) ? card.ImageUris[CardImageTypes.Small] : null,
                CroppedImage = card.ImageUris.ContainsKey(CardImageTypes.Border_Crop) ? card.ImageUris[CardImageTypes.Border_Crop] : null,
                Legalities1 = GetFirstHalfOfLegalities(source),
                Legalities2 = GetSecondHalfOfLegalities(source)
            }).ToList();

            return result;
        }

        private static FullCardResponse MapSingleFacedCardToDto(SfCard source)
        {
            var result = new FullCardResponse
            {
                Name = source.Name,
                CardId = source.CardId,
                PrintId = source.PrintId,
                LargeImage = source.ImageUris.ContainsKey(CardImageTypes.Large) ? source.ImageUris[CardImageTypes.Large] : null,
                SmallImage = source.ImageUris.ContainsKey(CardImageTypes.Small) ? source.ImageUris[CardImageTypes.Small] : null,
                CroppedImage = source.ImageUris.ContainsKey(CardImageTypes.Border_Crop) ? source.ImageUris[CardImageTypes.Border_Crop] : null,
                Legalities1 = GetFirstHalfOfLegalities(source),
                Legalities2 = GetSecondHalfOfLegalities(source),
                CardCount = 1,
                Text = GetTextOfCard(source),
                CardType = GetBaseCardTypeFromTypeLine(source.TypeLine)
            };

            return result;
        }

        private static Dictionary<string, string> GetFirstHalfOfLegalities(SfCard source)
        {
            return source.Legalities.Take(GetHalfIndexOfDictionary(source.Legalities)).ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' '));
        }

        private static Dictionary<string, string> GetSecondHalfOfLegalities(SfCard source)
        {
            return source.Legalities.Skip(GetHalfIndexOfDictionary(source.Legalities)).Take(source.Legalities.Count - GetHalfIndexOfDictionary(source.Legalities)).ToDictionary(k => k.Key.ToString(), e => e.Value.Replace('_', ' '));
        }

        private static int GetHalfIndexOfDictionary(Dictionary<MtgPlayModes, string> collection)
        {
            int count = collection.Count;
            if (count % 2 == 0)
                return count / 2;
            else
                return (count / 2) + 1;
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

        private static CardType GetBaseCardTypeFromTypeLine(string cardType)
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
    }
}
