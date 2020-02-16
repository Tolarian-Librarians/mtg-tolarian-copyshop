using System;
using System.Collections.Generic;

namespace Tolarian.Copyshop.Controller.Interfaces
{
    public interface IFullCard
    {
        string CardType { get; set; }

        Guid Id { get; set; }

        Dictionary<string, string> Legalities1 { get; set; }

        Dictionary<string, string> Legalities2 { get; set; }

        string Name { get; set; }

        Uri PngImage { get; set; }

        Uri SmallImage { get; set; }

        string Text { get; set; }
    }
}