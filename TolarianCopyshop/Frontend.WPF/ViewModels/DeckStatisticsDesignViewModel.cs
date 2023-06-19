using Tolarian.Copyshop.Controller;

namespace Tolarian.Copyshop.Fontend.WPF.ViewModels
{
    public class DeckStatisticsDesignViewModel : DeckStatisticsViewModel
    {
        public DeckStatisticsDesignViewModel() : this(null) { }

        public DeckStatisticsDesignViewModel(DeckController deckController)
            : base(deckController) { }
    }
}