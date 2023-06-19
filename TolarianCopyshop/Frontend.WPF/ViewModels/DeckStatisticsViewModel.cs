using System;
using System.Collections.Generic;
using System.Linq;

using LiveCharts;

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
            _deckController = deckController;

            PieChartPointLabel = chartPoint => chartPoint.Y.ToString();
            BarChartPointLabel = chartPoint => chartPoint.Y.ToString();
            XAxisLabelFormatter = val => val.ToString();
            YAxisLabelFormatter = val => val.ToString();

            ManaCurveCreatureCollection = new ChartValues<float>();
            ManaCurveNonCreatureCollection = new ChartValues<float>();

            BlackCardCollection = new ChartValues<float>();
            WhiteCardCollection = new ChartValues<float>();
            GreenCardCollection = new ChartValues<float>();
            BlueCardCollection = new ChartValues<float>();
            RedCardCollection = new ChartValues<float>();
            ColorlessCardCollection = new ChartValues<float>();
            MulticolorCardCollection = new ChartValues<float>();

            BlackManaCollection = new ChartValues<float>();
            WhiteManaCollection = new ChartValues<float>();
            RedManaCollection = new ChartValues<float>();
            BlueManaCollection = new ChartValues<float>();
            GreenManaCollection = new ChartValues<float>();

            LandCardTypCount = new ChartValues<float>();
            CreatureCardTypCount = new ChartValues<float>();
            InstantCardTypCount = new ChartValues<float>();
            SorceryCardTypCount = new ChartValues<float>();
            EnchantmentCardTypCount = new ChartValues<float>();
            ArtifactCardTypCount = new ChartValues<float>();
            PlaneswalkerCardTypCount = new ChartValues<float>();
        }

        #endregion

        #region Properties

        public ChartValues<float> ManaCurveCreatureCollection
        {
            get => _manaCurveCreatureCollection;
            set => SetProperty(ref _manaCurveCreatureCollection, value);
        }

        public ChartValues<float> ManaCurveNonCreatureCollection
        {
            get => _manaCurveNonCreatureCollection;
            set => SetProperty(ref _manaCurveNonCreatureCollection, value);
        }

        public ChartValues<float> BlackCardCollection
        {
            get => _blackCardCollection;
            set => SetProperty(ref _blackCardCollection, value);
        }

        public ChartValues<float> WhiteCardCollection
        {
            get => _whiteCardCollection;
            set => SetProperty(ref _whiteCardCollection, value);
        }

        public ChartValues<float> RedCardCollection
        {
            get => _redCardCollection;
            set => SetProperty(ref _redCardCollection, value);
        }

        public ChartValues<float> GreenCardCollection
        {
            get => _greenCardCollection;
            set => SetProperty(ref _greenCardCollection, value);
        }

        public ChartValues<float> BlueCardCollection
        {
            get => _blueCardCollection;
            set => SetProperty(ref _blueCardCollection, value);
        }

        public ChartValues<float> ColorlessCardCollection
        {
            get => _colorlessCardCollection;
            set => SetProperty(ref _colorlessCardCollection, value);
        }

        public ChartValues<float> MulticolorCardCollection
        {
            get => _multicolorCardCollection;
            set => SetProperty(ref _multicolorCardCollection, value);
        }

        public ChartValues<float> BlackManaCollection
        {
            get => _blackManaCollection;
            set => SetProperty(ref _blackManaCollection, value);
        }

        public ChartValues<float> WhiteManaCollection
        {
            get => _whiteManaCollection;
            set => SetProperty(ref _whiteManaCollection, value);
        }

        public ChartValues<float> RedManaCollection
        {
            get => _redManaCollection;
            set => SetProperty(ref _redManaCollection, value);
        }

        public ChartValues<float> BlueManaCollection
        {
            get => _blueManaCollection;
            set => SetProperty(ref _blueManaCollection, value);
        }

        public ChartValues<float> GreenManaCollection
        {
            get => _greenManaCollection;
            set => SetProperty(ref _greenManaCollection, value);
        }

        public ChartValues<float> LandCardTypCount
        {
            get => _landCardTypCount;
            set => SetProperty(ref _landCardTypCount, value);
        }

        public ChartValues<float> CreatureCardTypCount
        {
            get => _creatureCardTypCount;
            set => SetProperty(ref _creatureCardTypCount, value);
        }

        public ChartValues<float> InstantCardTypCount
        {
            get => _instantCardTypCount;
            set => SetProperty(ref _instantCardTypCount, value);
        }

        public ChartValues<float> SorceryCardTypCount
        {
            get => _sorceryCardTypCount;
            set => SetProperty(ref _sorceryCardTypCount, value);
        }

        public ChartValues<float> EnchantmentCardTypCount
        {
            get => _enchantmentCardTypCount;
            set => SetProperty(ref _enchantmentCardTypCount, value);
        }

        public ChartValues<float> ArtifactCardTypCount
        {
            get => _artifactCardTypCount;
            set => SetProperty(ref _artifactCardTypCount, value);
        }

        public ChartValues<float> PlaneswalkerCardTypCount
        {
            get => _planeswalkerCardTypCount;
            set => SetProperty(ref _planeswalkerCardTypCount, value);
        }

        public int CreatureCount
        {
            get => _creatureCount;
            set => SetProperty(ref _creatureCount, value);
        }

        public int NonCreatureCount
        {
            get => _nonCreatureCount;
            set => SetProperty(ref _nonCreatureCount, value);
        }

        public int TotalCards
        {
            get => _totalCards;
            set => SetProperty(ref _totalCards, value);
        }

        public float AverageCmc
        {
            get => _averageCmc;
            set => SetProperty(ref _averageCmc, value);
        }

        public Func<ChartPoint, string> BarChartPointLabel { get; }

        public Func<ChartPoint, string> PieChartPointLabel { get; }

        public Func<ChartPoint, string> XAxisLabelFormatter { get; }

        public Func<ChartPoint, string> YAxisLabelFormatter { get; }

        #endregion

        #region Methods

        public void LoadChartData()
        {
            GetDeckStatisticsResponse result = _deckController.GetDeckStatistics(DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());

            ManaCurveCreatureCollection = new ChartValues<float>();

            ManaCurveCreatureCollection = AddValuesToBarChart(result.ManaCurveCreatures);
            ManaCurveNonCreatureCollection = AddValuesToBarChart(result.ManaCurveNonCreatures);

            BlackCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.B],
            };
            WhiteCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.W],
            };
            BlueCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.U],
            };
            RedCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.R],
            };
            GreenCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.G],
            };
            ColorlessCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.C],
            };
            MulticolorCardCollection = new ChartValues<float>
            {
                result.ColorCardsCounts[MtgColor.M],
            };

            BlackManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.B],
                result.ColorSymbolCounts[MtgColor.B],
            };
            BlueManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.U],
                result.ColorSymbolCounts[MtgColor.U],
            };
            GreenManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.G],
                result.ColorSymbolCounts[MtgColor.G],
            };
            RedManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.R],
                result.ColorSymbolCounts[MtgColor.R],
            };
            WhiteManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[MtgColor.W],
                result.ColorSymbolCounts[MtgColor.W],
            };

            LandCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Land],
            };
            CreatureCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Creature],
            };
            InstantCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Instant],
            };
            SorceryCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Sorcery],
            };
            EnchantmentCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Enchantment],
            };
            ArtifactCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Artifact],
            };
            PlaneswalkerCardTypCount = new ChartValues<float>()
            {
                result.CardTypeCounts[CardType.Planeswalker],
            };

            TotalCards = result.TotalCards;
            CreatureCount = result.CreatureCount;
            NonCreatureCount = result.NonCreatureCount;
            AverageCmc = result.AverageCmc;
        }

        private ChartValues<float> AddValuesToBarChart(Dictionary<float, int> newValues)
        {
            ChartValues<float> chartValue = new();
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