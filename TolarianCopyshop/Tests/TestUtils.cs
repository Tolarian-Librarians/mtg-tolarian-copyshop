using System;
using System.Collections.Generic;

using Tolarian.Copyshop.Business.Models.Enums;
using Tolarian.Copyshop.Business.Models.SfCardInfo;

namespace Tests
{
    public static class TestUtils
    {
        public static SfCard GetDummyDualFacedCard()
        {
            SfCard card = GetDummyDoubleCard();
            card.ImageUris = null;

            return card;
        }

        public static SfCard GetDummyDoubleCard()
        {
            SfCard card = GetDummyCard();
            card.Text = null;
            card.CardFaces = new List<SfCardFace> {
                new SfCardFace
                {
                    Name = "Face 1",
                    Text = "Text 1",
                    TypeLine = "Artifact Creature - Dummy",
                    ImageUris = new Dictionary<CardImageTypes, Uri>() ,
                },
                new SfCardFace
                {
                    Name = "Face 2",
                    Text = "Text 2",
                    TypeLine = "Artifact Creature - Dummy",
                    ImageUris = new Dictionary<CardImageTypes, Uri>(),
                }
            };

            return card;
        }

        public static SfCard GetDummyCard()
        {
            return new SfCard
            {
                Name = "Dummy Mtg Card",
                CardId = Guid.NewGuid(),
                PrintId = Guid.NewGuid(),
                ImageUris = new Dictionary<CardImageTypes, Uri> {
                    { CardImageTypes.Normal, new Uri("https://img.scryfall.com/cards/png/front/e/6/e672d408-997c-4a19-810a-3da8411eecf2.png?1568004958") },
                    { CardImageTypes.Small, new Uri("https://img.scryfall.com/cards/small/front/e/6/e672d408-997c-4a19-810a-3da8411eecf2.jpg?1568004958") },
                    { CardImageTypes.Border_Crop, new Uri("https://img.scryfall.com/cards/border_crop/front/0/e/0e0d989d-7186-40dc-bdfe-cfbb77656bc8.jpg?1559959218") } },
                Legalities = new Dictionary<string, string> {
                    { "commander", "legal" },
                    { "brawl", "legal" },
                    { "duel", "legal" },
                    { "future", "legal" },
                    { "historic", "legal" },
                    { "legacy", "legal" },
                    { "modern", "legal" } },
                Text = "Does dummy stuff.",
                TypeLine = "Artifact Creature - Dummy"
            };
        }

        public static SfPaginatedCardList GetDummyCardList()
        {
            return new SfPaginatedCardList
            {
                CardCount = 5,
                Data = new SfCard[]
                {
                    new SfCard(),
                    new SfCard(),
                    new SfCard(),
                    new SfCard(),
                    new SfCard(),
                }
            };
        }

        public static SfCardCollection GetDummyCardCollection()
        {
            return new SfCardCollection
            {
                NotFound = Array.Empty<SfIdentifier>(),
                Data = new SfCard[]
                {
                    new SfCard(),
                    new SfCard(),
                    new SfCard(),
                    new SfCard(),
                    new SfCard(),
                }
            };
        }
    }
}