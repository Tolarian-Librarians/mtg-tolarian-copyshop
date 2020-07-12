using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.Controller;
using Tolarian.Copyshop.Controller.Interfaces;
using Tolarian.Copyshop.Controller.ResponseObjects;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckStatisticsViewModel : BindableBase
    {
        private ChartValues<float> _manaCurveCollection;
        private ChartValues<float> _blackManaCollection;
        private ChartValues<float> _whiteManaCollection;
        private ChartValues<float> _redManaCollection;
        private ChartValues<float> _blueManaCollection;
        private ChartValues<float> _greenManaCollection;
        //private ChartValues<float> _colorlessManaCollection;
        private ChartValues<float> _landCardTypCount;
        private ChartValues<float> _creatureCardTypCount;
        private ChartValues<float> _instantCardTypCount;
        private ChartValues<float> _sorceryCardTypCount;
        private ChartValues<float> _enchantmentCardTypCount;
        private ChartValues<float> _artifactCardTypCount;
        private ChartValues<float> _planeswalkerCardTypCount;
        private readonly DeckController _deckController;

        public DeckStatisticsViewModel(DeckController deckController)
        {
            this._deckController = deckController;

            this.ChartPointLabel = this.ChartPoint;

            this.ManaCurveCollection = new ChartValues<float>();
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

        public ChartValues<float> ManaCurveCollection
        {
            get => this._manaCurveCollection;
            set => this.SetProperty(ref this._manaCurveCollection, value);
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

        //public ChartValues<float> ColorlessManaCollection
        //{
        //    get => this._colorlessManaCollection;
        //    set => this.SetProperty(ref this._colorlessManaCollection, value);
        //}

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

        public Func<ChartPoint, string> ChartPointLabel { get; }

        #region Methods

        private string ChartPoint(ChartPoint chartPoint)
            => $"{chartPoint.Y} ({chartPoint.Participation:P})";

        public void LoadChartData()
        {
            GetDeckStatisticsResponse result = this._deckController.GetDeckStatistics(DeckBuilderViewModel.GetInstance().DeckCards.Cast<IFullCard>().ToList());

            this.ManaCurveCollection = new ChartValues<float>();
            foreach (KeyValuePair<float, int> keyValuePair in result.ManaCurve)
            {
                this.ManaCurveCollection.Add(keyValuePair.Value);
            }

            if (this.ManaCurveCollection.Count == 0)
            {
                this.ManaCurveCollection.Add(0);
            }

            this.BlackManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[Controller.ResponseObjects.Enums.MtgColor.B],
                result.ColorSymbolCounts[Controller.ResponseObjects.Enums.MtgColor.B],
            };
            this.BlueManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[Controller.ResponseObjects.Enums.MtgColor.U],
                result.ColorSymbolCounts[Controller.ResponseObjects.Enums.MtgColor.U],
            };
            this.GreenManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[Controller.ResponseObjects.Enums.MtgColor.G],
                result.ColorSymbolCounts[Controller.ResponseObjects.Enums.MtgColor.G],
            };
            this.RedManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[Controller.ResponseObjects.Enums.MtgColor.R],
                result.ColorSymbolCounts[Controller.ResponseObjects.Enums.MtgColor.R],
            };
            this.WhiteManaCollection = new ChartValues<float>
            {
                result.ManaSourcesCounts[Controller.ResponseObjects.Enums.MtgColor.W],
                result.ColorSymbolCounts[Controller.ResponseObjects.Enums.MtgColor.W],
            };
        }

        #endregion
    }
}
