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
            this.ChartPointLabel = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})";
            this.XAxisLabelFormatter = val => val.ToString();
            this.YAxisLabelFormatter = val => val.ToString();
        }

        public Func<ChartPoint, string> ChartPointLabel { get; }

        public Func<ChartPoint, string> XAxisLabelFormatter { get; set; }

        public Func<ChartPoint, string> YAxisLabelFormatter { get; set; }
    }
}
