using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.Controller.ResponseObjects.Enums;
using Tolarian.Copyshop.Fontend.WPF.Base;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class DeckStatisticsViewModel : BindableBase
    {
        #region Fields

        private int _creatureCount;
        private int _nonCreatureCount;
        private int _totalCards;
        private float _averageCmc;
        private ChartValues<float> _blackCardCollection;
        private ChartValues<float> _whiteCardCollection;
        private ChartValues<float> _redCardCollection;
        private ChartValues<float> _greenCardCollection;
        private ChartValues<float> _blueCardCollection;
        private ChartValues<float> _colorlessCardCollection;
        private ChartValues<float> _multicolorCardCollection;
        private ChartValues<float> _manaCurveCreatureCollection;
        private ChartValues<float> _manaCurveNonCreatureCollection;
        private ChartValues<float> _blackManaCollection;
        private ChartValues<float> _whiteManaCollection;
        private ChartValues<float> _redManaCollection;
        private ChartValues<float> _blueManaCollection;
        private ChartValues<float> _greenManaCollection;
        private ChartValues<float> _landCardTypCount;
        private ChartValues<float> _creatureCardTypCount;
        private ChartValues<float> _instantCardTypCount;
        private ChartValues<float> _sorceryCardTypCount;
        private ChartValues<float> _enchantmentCardTypCount;
        private ChartValues<float> _artifactCardTypCount;
        private ChartValues<float> _planeswalkerCardTypCount;
        private readonly DeckController _deckController;

        #endregion

        #region ctor

        public DeckStatisticsViewModel(DeckController deckController)
        {
            this._deckController = deckController;

            this.PieChartPointLabel = chartPoint => chartPoint.Y.ToString();
            this.BarChartPointLabel = chartPoint => chartPoint.Y.ToString();
            this.XAxisLabelFormatter = val => val.ToString();
            this.YAxisLabelFormatter = val => val.ToString();

            this.ManaCurveCreatureCollection = new ChartValues<float>();
            this.ManaCurveNonCreatureCollection = new ChartValues<float>();

            this.BlackCardCollection = new ChartValues<float>();
            this.WhiteCardCollection = new ChartValues<float>();
            this.GreenCardCollection = new ChartValues<float>();
            this.BlueCardCollection = new ChartValues<float>();
            this.RedCardCollection = new ChartValues<float>();
            this.ColorlessCardCollection = new ChartValues<float>();
            this.MulticolorCardCollection = new ChartValues<float>();

            this.BlackManaCollection = new ChartValues<float>();
            this.WhiteManaCollection = new ChartValues<float>();
            this.RedManaCollection = new ChartValues<float>();
            this.BlueManaCollection = new ChartValues<float>();
            this.GreenManaCollection = new ChartValues<float>();

            this.LandCardTypCount = new ChartValues<float>();
            this.CreatureCardTypCount = new ChartValues<float>();
            this.InstantCardTypCount = new ChartValues<float>();
            this.SorceryCardTypCount = new ChartValues<float>();
            this.EnchantmentCardTypCount = new ChartValues<float>();
            this.ArtifactCardTypCount = new ChartValues<float>();
            this.PlaneswalkerCardTypCount = new ChartValues<float>();
        }

        #endregion

        #region Properties

        public ChartValues<float> ManaCurveCreatureCollection
        {
            get => this._manaCurveCreatureCollection;
            set => this.SetProperty(ref this._manaCurveCreatureCollection, value);
        }

        public ChartValues<float> ManaCurveNonCreatureCollection
        {
            get => this._manaCurveNonCreatureCollection;
            set => this.SetProperty(ref this._manaCurveNonCreatureCollection, value);
        }

        public ChartValues<float> BlackCardCollection
        {
            get => this._blackCardCollection;
            set => this.SetProperty(ref this._blackCardCollection, value);
        }

        public ChartValues<float> WhiteCardCollection
        {
            get => this._whiteCardCollection;
            set => this.SetProperty(ref this._whiteCardCollection, value);
        }

        public ChartValues<float> RedCardCollection
        {
            get => this._redCardCollection;
            set => this.SetProperty(ref this._redCardCollection, value);
        }

        public ChartValues<float> GreenCardCollection
        {
            get => this._greenCardCollection;
            set => this.SetProperty(ref this._greenCardCollection, value);
        }

        public ChartValues<float> BlueCardCollection
        {
            get => this._blueCardCollection;
            set => this.SetProperty(ref this._blueCardCollection, value);
        }

        public ChartValues<float> ColorlessCardCollection
        {
            get => this._colorlessCardCollection;
            set => this.SetProperty(ref this._colorlessCardCollection, value);
        }

        public ChartValues<float> MulticolorCardCollection
        {
            get => this._multicolorCardCollection;
            set => this.SetProperty(ref this._multicolorCardCollection, value);
        }

        public ChartValues<float> BlackManaCollection
        {
            get => this._blackManaCollection;
            set => this.SetProperty(ref this._blackManaCollection, value);
        }

        public ChartValues<float> WhiteManaCollection
        {
            get => this._whiteManaCollection;
            set => this.SetProperty(ref this._whiteManaCollection, value);
        }

        public ChartValues<float> RedManaCollection
        {
            get => this._redManaCollection;
            set => this.SetProperty(ref this._redManaCollection, value);
        }

        public ChartValues<float> BlueManaCollection
        {
            get => this._blueManaCollection;
            set => this.SetProperty(ref this._blueManaCollection, value);
        }

        public ChartValues<float> GreenManaCollection
        {
            get => this._greenManaCollection;
            set => this.SetProperty(ref this._greenManaCollection, value);
        }

        public ChartValues<float> LandCardTypCount
        {
            get => this._landCardTypCount;
            set => this.SetProperty(ref this._landCardTypCount, value);
        }

        public ChartValues<float> CreatureCardTypCount
        {
            get => this._creatureCardTypCount;
            set => this.SetProperty(ref this._creatureCardTypCount, value);
        }

        public ChartValues<float> InstantCardTypCount
        {
            get => this._instantCardTypCount;
            set => this.SetProperty(ref this._instantCardTypCount, value);
        }

        public ChartValues<float> SorceryCardTypCount
        {
            get => this._sorceryCardTypCount;
            set => this.SetProperty(ref this._sorceryCardTypCount, value);
        }

        public ChartValues<float> EnchantmentCardTypCount
        {
            get => this._enchantmentCardTypCount;
            set => this.SetProperty(ref this._enchantmentCardTypCount, value);
        }

        public ChartValues<float> ArtifactCardTypCount
        {
            get => this._artifactCardTypCount;
            set => this.SetProperty(ref this._artifactCardTypCount, value);
        }

        public ChartValues<float> PlaneswalkerCardTypCount
        {
            get => this._planeswalkerCardTypCount;
            set => this.SetProperty(ref this._planeswalkerCardTypCount, value);
        }

        public int CreatureCount
        {
            get => this._creatureCount;
            set => this.SetProperty(ref this._creatureCount, value);
        }

        public int NonCreatureCount
        {
            get => this._nonCreatureCount;
            set => this.SetProperty(ref this._nonCreatureCount, value);
        }

        public int TotalCards
        {
            get => this._totalCards;
            set => this.SetProperty(ref this._totalCards, value);
        }

        public float AverageCmc
        {
            get => this._averageCmc;
            set => this.SetProperty(ref this._averageCmc, value);
        }

        public Func<ChartPoint, string> BarChartPointLabel { get; }

        public Func<ChartPoint, string> PieChartPointLabel { get; }

        public Func<ChartPoint, string> XAxisLabelFormatter { get; }

        public Func<ChartPoint, string> YAxisLabelFormatter { get; }

        #endregion

        #region Methods

        public void LoadChartData()
        {
            GetDeckStatisticsResponse result = this._deckController.GetDeckStatistics(DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());

            this.ManaCurveCreatureCollection = new ChartValues<float>();

            this.ManaCurveCreatureCollection = this.AddValuesToBarChart(result.ManaCurveCreatures);
            this.ManaCurveNonCreatureCollection = this.AddValuesToBarChart(result.ManaCurveNonCreatures);

            this.BlackCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.B],
            };
            this.WhiteCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.W],
            };
            this.BlueCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.U],
            };
            this.RedCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.R],
            };
            this.GreenCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.G],
            };
            this.ColorlessCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.C],
            };
            this.MulticolorCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.M],
            };

            this.BlackManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.B],
                result.ColorSymbolCounts[MtgColor.B],
            };
            this.BlueManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.U],
                result.ColorSymbolCounts[MtgColor.U],
            };
            this.GreenManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.G],
                result.ColorSymbolCounts[MtgColor.G],
            };
            this.RedManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.R],
                result.ColorSymbolCounts[MtgColor.R],
            };
            this.WhiteManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.W],
                result.ColorSymbolCounts[MtgColor.W],
            };

            this.LandCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Land],
            };
            this.CreatureCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Creature],
            };
            this.InstantCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Instant],
            };
            this.SorceryCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Sorcery],
            };
            this.EnchantmentCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Enchantment],
            };
            this.ArtifactCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Artifact],
            };
            this.PlaneswalkerCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Planeswalker],
            };

            this.TotalCards = result.TotalCards;
            this.CreatureCount = result.CreatureCount;
            this.NonCreatureCount = result.NonCreatureCount;
            this.AverageCmc = result.AverageCmc;
        }

        private ChartValues<float> AddValuesToBarChart(Dictionary<float, int> newValues)
        {
            ChartValues<float> chartValue = new ChartValues<float>();
            foreach (KeyValuePair<float, int> keyValuePair in newValues)
            {
                while (chartValue.Count < keyValuePair.Key)
                {
                    chartValue.Add(0);
                }

                chartValue.Add(keyValuePair.Value);
            }
            return chartValue;
        }

        #endregion
    }
}
