using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.Interfaces
{
    public interface IFullCard
    {
        CardType CardType { get; set; }
        int CardCount { get; set; }
        Guid CardId { get; set; }
        Guid PrintId { get; set; }
        Dictionary<string, string> Legalities1 { get; set; }
        Dictionary<string, string> Legalities2 { get; set; }
        string Name { get; set; }
        Uri LargeImage { get; set; }
        Uri SmallImage { get; set; }
        string Text { get; set; }
        string SetCode { get; set; }

    }
}