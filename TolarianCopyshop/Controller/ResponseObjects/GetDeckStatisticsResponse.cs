using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class GetDeckStatisticsResponse
    {
        public Dictionary<MtgColor, int> ColorSymbolCounts { get; set; }
        public Dictionary<MtgColor, int> ColorCardsCounts { get; set; }
        public Dictionary<MtgColor, int> ManaSourcesCounts { get; set; }
        public Dictionary<CardType, int> CardTypeCounts { get; set; }
        public Dictionary<float, int> ManaCurveCreatures { get; set; }
        public Dictionary<float, int> ManaCurveNonCreatures { get; set; }
        public int CreatureCount { get; set; }
        public int NonCreatureCount { get; set; }
        public float AverageCmc { get; set; }
        public int TotalCards { get; set; }


        public static GetDeckStatisticsResponse Empty()
            => new GetDeckStatisticsResponse()
            {
                ColorSymbolCounts = new Dictionary<MtgColor, int>(),
                ColorCardsCounts = new Dictionary<MtgColor, int>(),
                ManaSourcesCounts = new Dictionary<MtgColor, int>(),
                CardTypeCounts = new Dictionary<CardType, int>(),
                ManaCurveNonCreatures = new Dictionary<float, int>(),
                ManaCurveCreatures = new Dictionary<float, int>(),
                AverageCmc = 0,
                CreatureCount = 0,
                NonCreatureCount = 0,
                TotalCards = 0,
            };
    }
}
