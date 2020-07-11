using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckStatisticsDesignViewModel
    {
        public DeckStatisticsDesignViewModel()
        {
            this.ChartPointLabel = this.ChartPoint;
        }

        public Func<ChartPoint, string> ChartPointLabel { get; }

        private string ChartPoint(ChartPoint chartPoint)
            => $"{chartPoint.Y} ({chartPoint.Participation:P})";
    }
}
