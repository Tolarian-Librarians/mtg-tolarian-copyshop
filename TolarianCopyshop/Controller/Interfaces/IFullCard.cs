using System;
using System.Collections.Generic;

using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.Interfaces
{
    public interface IFullCard
    {
        int CardCount { get; set; }
        Guid CardId { get; set; }
        string FormattedCardName { get; set; }
        Guid PrintId { get; set; }
        Dictionary<string, string> Legalities { get; set; }
        string SetCode { get; set; }
        string ManaCostLine { get; set; }
        float ConvertedManaCost { get; set; }
        List<MtgColor> ColorIdentity { get; set; }
        List<MtgColor> Colors { get; set; }
        List<MtgColor> ProducedMana { get; set; }
        ICollection<CardFace> CardFaces { get; set; }
        ICollection<RelatedCard> RelatedCards { get; set; }
        bool IsTransformable { get; set; }
    }
}