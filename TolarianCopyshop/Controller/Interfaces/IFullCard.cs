using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.ResponseObjects;

namespace Tolarian.Copyshop.Controller.Interfaces
{
    public interface IFullCard
    {
        int CardCount { get; set; }
        Guid CardId { get; set; }
        string FormattedCardName { get; set; }
        Guid PrintId { get; set; }
        Dictionary<string, string> Legalities1 { get; set; }
        Dictionary<string, string> Legalities2 { get; set; }
        string SetCode { get; set; }
        ICollection<CardFace> CardFaces { get; set; }
        bool IsTransformable { get; set; }
    }
}