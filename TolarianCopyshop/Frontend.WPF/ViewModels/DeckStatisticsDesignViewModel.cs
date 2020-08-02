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
            this.PieChartPointLabel = chartPoint => chartPoint.Y.ToString();
            this.BarChartPointLabel = chartPoint => chartPoint.Y.ToString();
            this.XAxisLabelFormatter = val => val.ToString();
            this.YAxisLabelFormatter = val => val.ToString();
        }

        public Func<ChartPoint, string> BarChartPointLabel { get; }

        public Func<ChartPoint, string> PieChartPointLabel { get; }

        public Func<ChartPoint, string> XAxisLabelFormatter { get; }

        public Func<ChartPoint, string> YAxisLabelFormatter { get; }
    }
}
