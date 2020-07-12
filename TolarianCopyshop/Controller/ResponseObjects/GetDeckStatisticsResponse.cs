using System;
using System.Collections.Generic;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;

namespace Tolarian.Copyshop.Controller.ResponseObjects
{
    public class GetDeckStatisticsResponse
    {
        public Dictionary<MtgColor, int> ColorSymbolCounts { get; set; }
        public Dictionary<MtgColor, int> ManaSourcesCounts { get; set; }
        public Dictionary<CardType, int> CardTypeCounts { get; set; }
        public Dictionary<float, int> ManaCurve { get; set; }
    }
}
