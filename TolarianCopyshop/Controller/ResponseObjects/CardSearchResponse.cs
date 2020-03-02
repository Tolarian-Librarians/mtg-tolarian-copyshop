using System;
using System.Collections.Generic;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class CardSearchResponse
    {
        public int ResultsCount { get; set; }
        public List<CardSearchCard> Results { get; set; }
    }
}
