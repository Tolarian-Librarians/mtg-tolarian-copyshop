using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tolarian.Copyshop.ScreenPresenter.Base;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckStatisticsViewModel : BindableBase
    {
        private ObservableCollection<float> _manaCurveCollection;
        private ObservableCollection<float> _blackManaCollection;
        private ObservableCollection<float> _whiteManaCollection;
        private ObservableCollection<float> _redManaCollection;
        private ObservableCollection<float> _blueManaCollection;
        private ObservableCollection<float> _greenManaCollection;
        private ObservableCollection<float> _colorlessManaCollection;
        private float _landCardTypCount;
        private float _creatureCardTypCount;
        private float _instantCardTypCount;
        private float _sorceryCardTypCount;
        private float _enchantmentCardTypCount;
        private float _artifactCardTypCount;
        private float _planeswalkerCardTypCount;

        public DeckStatisticsViewModel()
        {
            this.ChartPointLabel = this.ChartPoint;
        }

        public ObservableCollection<float> ManaCurveCollection
        {
            get => this._manaCurveCollection;
            set => this.SetProperty(ref this._manaCurveCollection, value);
        }

        public ObservableCollection<float> BlackManaCollection
        {
            get => this._blackManaCollection;
            set => this.SetProperty(ref this._blackManaCollection, value);
        }

        public ObservableCollection<float> WhiteManaCollection
        {
            get => this._whiteManaCollection;
            set => this.SetProperty(ref this._whiteManaCollection, value);
        }

        public ObservableCollection<float> RedManaCollection
        {
            get => this._redManaCollection;
            set => this.SetProperty(ref this._redManaCollection, value);
        }

        public ObservableCollection<float> BlueManaCollection
        {
            get => this._blueManaCollection;
            set => this.SetProperty(ref this._blueManaCollection, value);
        }

        public ObservableCollection<float> GreenManaCollection
        {
            get => this._greenManaCollection;
            set => this.SetProperty(ref this._greenManaCollection, value);
        }

        public ObservableCollection<float> ColorlessManaCollection
        {
            get => this._colorlessManaCollection;
            set => this.SetProperty(ref this._colorlessManaCollection, value);
        }

        public float LandCardTypCount
        {
            get => this._landCardTypCount;
            set => this.SetProperty(ref this._landCardTypCount, value);
        }

        public float CreatureCardTypCount
        {
            get => this._creatureCardTypCount;
            set => this.SetProperty(ref this._creatureCardTypCount, value);
        }

        public float InstantCardTypCount
        {
            get => this._instantCardTypCount;
            set => this.SetProperty(ref this._instantCardTypCount, value);
        }

        public float SorceryCardTypCount
        {
            get => this._sorceryCardTypCount;
            set => this.SetProperty(ref this._sorceryCardTypCount, value);
        }

        public float EnchantmentCardTypCount
        {
            get => this._enchantmentCardTypCount;
            set => this.SetProperty(ref this._enchantmentCardTypCount, value);
        }

        public float ArtifactCardTypCount
        {
            get => this._artifactCardTypCount;
            set => this.SetProperty(ref this._artifactCardTypCount, value);
        }

        public float PlaneswalkerCardTypCount
        {
            get => this._planeswalkerCardTypCount;
            set => this.SetProperty(ref this._planeswalkerCardTypCount, value);
        }

        public Func<ChartPoint, string> ChartPointLabel { get; }

        #region Methods

        private string ChartPoint(ChartPoint chartPoint)
            => $"{chartPoint.Y} ({chartPoint.Participation:P})";

        #endregion
    }
}
