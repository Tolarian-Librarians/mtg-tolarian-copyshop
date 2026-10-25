using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;

using Tolarian.Copyshop.Business.Entities;
using Tolarian.Copyshop.Business.Interfaces;
using Tolarian.Copyshop.Business.UseCaseInteractors;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;
using Tolarian.Copyshop.ScryfallDataAccess;

namespace Tests.ScryfallApiChangeTests
{
    /// <summary>
    /// Scryfall regularly adds new values to fields we model as enums (image types, formats, colors, related card components).
    /// These tests feed real Scryfall card JSON with such additions through the whole stack below the UI
    /// (controller, interactors, Refit/Newtonsoft data access) and make sure cards still load.
    /// </summary>
    [TestClass]
    public class ScryfallApiChangeTests
    {
        private const string YoungPyromancer = "young-pyromancer-m14.json";
        private const string DelverOfSecrets = "delver-of-secrets-isd.json";
        private const string AdrixAndNevReversible = "adrix-and-nev-reversible-sld.json";

        [TestMethod]
        public void LoadCard_WithUnknownImageType_StillLoadsCard()
        {
            JObject json = FakeScryfallHandler.LoadCard(YoungPyromancer);
            json["image_uris"]["hologram"] = "https://cards.scryfall.io/hologram/front/e/3/e349c204.webp";

            IFullCard card = LoadCardByPrintId(json);

            Assert.AreEqual(new Uri(json["image_uris"]["display"].ToString()), card.CardFaces.Single().LargeImage);
        }

        [TestMethod]
        public void LoadCard_WithUnknownImageTypeOnCardFace_StillLoadsCard()
        {
            JObject json = FakeScryfallHandler.LoadCard(DelverOfSecrets);
            json["card_faces"][1]["image_uris"]["hologram"] = "https://cards.scryfall.io/hologram/back/1/1/11bf83bb.webp";

            IFullCard card = LoadCardByPrintId(json);

            Assert.HasCount(2, card.CardFaces);
            Assert.AreEqual(new Uri(json["card_faces"][1]["image_uris"]["display"].ToString()), card.CardFaces.Last().LargeImage);
        }

        [TestMethod]
        public void LoadCard_WithUnknownColors_IgnoresUnknownColors()
        {
            JObject json = FakeScryfallHandler.LoadCard(YoungPyromancer);
            json["colors"] = new JArray("R", "P");
            json["color_identity"] = new JArray("R", "P");
            json["produced_mana"] = new JArray("R", "P");

            IFullCard card = LoadCardByPrintId(json);

            CollectionAssert.AreEqual(new List<MtgColor> { MtgColor.R }, card.Colors);
            CollectionAssert.AreEqual(new List<MtgColor> { MtgColor.R }, card.ColorIdentity);
            CollectionAssert.AreEqual(new List<MtgColor> { MtgColor.R }, card.ProducedMana);
            CollectionAssert.AreEqual(new List<MtgColor> { MtgColor.R }, card.CardFaces.Single().Colors);
        }

        [TestMethod]
        public void LoadCard_WithUnknownColorsOnCardFace_IgnoresUnknownColors()
        {
            JObject json = FakeScryfallHandler.LoadCard(DelverOfSecrets);
            json["card_faces"][0]["colors"] = new JArray("U", "P");

            IFullCard card = LoadCardByPrintId(json);

            CollectionAssert.AreEqual(new List<MtgColor> { MtgColor.U }, card.CardFaces.First().Colors);
        }

        [TestMethod]
        public void LoadCard_WithUnknownRelatedCardComponent_KeepsKnownRelatedCards()
        {
            JObject json = FakeScryfallHandler.LoadCard(YoungPyromancer);
            JToken tokenPart = json["all_parts"].Single(p => p["component"].ToString() == "token");
            ((JArray)json["all_parts"]).Add(new JObject
            {
                ["object"] = "related_card",
                ["id"] = "00000000-0000-0000-0000-000000000001",
                ["component"] = "attraction",
                ["name"] = "Some Attraction",
            });

            IFullCard card = LoadCardByPrintId(json);

            Assert.HasCount(2, card.RelatedCards);
            Assert.ContainsSingle(card.RelatedCards.Where(rc => rc.Type == RelatedCardType.token && rc.Id == Guid.Parse(tokenPart["id"].ToString())));
        }

        [TestMethod]
        public void ImportDeck_WithUnknownValuesInEveryEnumField_ImportsCard()
        {
            JObject json = FakeScryfallHandler.LoadCard(YoungPyromancer);
            json["image_uris"]["hologram"] = "https://cards.scryfall.io/hologram/front/e/3/e349c204.webp";
            json["colors"] = new JArray("R", "P");
            json["color_identity"] = new JArray("R", "P");
            json["produced_mana"] = new JArray("P");
            ((JArray)json["all_parts"]).Add(new JObject { ["id"] = Guid.NewGuid().ToString(), ["component"] = "attraction" });
            CardController controller = CreateController(new FakeScryfallHandler(json));

            CardImportResponse response = controller.GetCardsByImportString("1 Young Pyromancer");

            Assert.AreEqual(string.Empty, controller.GetErrorMessage() ?? string.Empty);
            Assert.AreEqual(string.Empty, response.NotFound);
            Assert.AreEqual("Young Pyromancer", response.Cards.Single().FormattedCardName);
        }

        [TestMethod]
        public void LoadCard_ReversibleCard_TakesOracleIdFromCardFace()
        {
            // Reversible cards (layout "reversible_card") have no top level oracle_id, only their faces have one.
            JObject json = FakeScryfallHandler.LoadCard(AdrixAndNevReversible);
            Assert.IsNull(json["oracle_id"]);
            Guid faceOracleId = Guid.Parse(json["card_faces"][0]["oracle_id"].ToString());

            IFullCard card = LoadCardByPrintId(json);

            Assert.AreEqual(faceOracleId, card.CardId);
        }

        [TestMethod]
        public void SelectArtwork_OfReversibleCard_RequestsPrintsOfItsOracleId()
        {
            JObject json = FakeScryfallHandler.LoadCard(AdrixAndNevReversible);
            string faceOracleId = json["card_faces"][0]["oracle_id"].ToString();
            FakeScryfallHandler handler = new FakeScryfallHandler(json);
            CardController controller = CreateController(handler);
            IFullCard card = controller.GetCardByPrintId(Guid.Parse(json["id"].ToString())).Card;

            CardArtworkResponse artworks = controller.GetArtworksOfCard(card.CardId);

            Assert.AreEqual(string.Empty, controller.GetErrorMessage() ?? string.Empty);
            Assert.HasCount(1, artworks.Artworks);
            Assert.Contains($"oracleid:{faceOracleId}", Uri.UnescapeDataString(handler.RequestedUris.Last().Query));
        }

        private static IFullCard LoadCardByPrintId(JObject json)
        {
            CardController controller = CreateController(new FakeScryfallHandler(json));

            FullCardResponse response = controller.GetCardByPrintId(Guid.Parse(json["id"].ToString()));

            Assert.AreEqual(string.Empty, controller.GetErrorMessage() ?? string.Empty);
            Assert.IsNotNull(response.Card);
            return response.Card;
        }

        private static CardController CreateController(FakeScryfallHandler handler)
        {
            CardDataMapper gateway = new CardDataMapper(handler);
            // Set codes are not used in these tests, so the set gateway must never be called.
            Mock<ISetDataGateway> setGateway = new Mock<ISetDataGateway>(MockBehavior.Strict);
            DeckImportInteractor importInteractor = new DeckImportInteractor(gateway, new SetCodeTranslator(setGateway.Object), new ImportStringParser());

            return new CardController(new CardInteractor(gateway), importInteractor);
        }
    }
}
