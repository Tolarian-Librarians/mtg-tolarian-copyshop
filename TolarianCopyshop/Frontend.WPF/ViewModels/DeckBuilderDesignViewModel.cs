using System;
using System.Collections.ObjectModel;
using System.Windows;
using Tolarian.Copyshop.ScreenPresenter.Model;

namespace Tolarian.Copyshop.ScreenPresenter.ViewModels
{
    public class DeckBuilderDesignViewModel
    {
        public ObservableCollection<FullCardModel> DeckCards { get; set; }

        public FullCardModel SelectedCard { get; set; }

        public Visibility SearchResultVisibility { get; set; } = Visibility.Hidden;

        public DeckBuilderDesignViewModel()
        {
            this.DeckCards = new ObservableCollection<FullCardModel>
            {
                new FullCardModel()
                {
                    CardId = new Guid(),
                    SmallImage = new Uri("https://img.scryfall.com/cards/small/front/3/2/32982ed2-96e4-4cc5-8562-744b06bca239.jpg?1572491355"),
                    LargeImage = new Uri("https://img.scryfall.com/cards/large/front/a/f/af951b7c-21ba-4a40-aa19-20059b8ca63f.jpg?1567840489"),
                    Name = "Mountain",
                    Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                },
                new FullCardModel()
                {
                    CardId = new Guid(),
                    SmallImage = new Uri("https://img.scryfall.com/cards/small/front/e/4/e4f184c5-4f3c-4aea-afa1-f0903d3cc71a.jpg?1572491327"),
                    LargeImage = new Uri("https://img.scryfall.com/cards/small/front/e/4/e4f184c5-4f3c-4aea-afa1-f0903d3cc71a.jpg?1572491327"),
                    Name = "Swamp",
                    Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                },
                new FullCardModel()
                {
                    CardId = new Guid(),
                    SmallImage = new Uri("https://img.scryfall.com/cards/small/front/7/d/7ded4b2a-ba56-43da-8ea7-392b77fc2926.jpg?1572491259"),
                    LargeImage = new Uri("https://img.scryfall.com/cards/small/front/7/d/7ded4b2a-ba56-43da-8ea7-392b77fc2926.jpg?1572491259"),
                    Name = "Plains",
                    Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                },
                new FullCardModel()
                {
                    CardId = new Guid(),
                    SmallImage = new Uri("https://img.scryfall.com/cards/small/front/4/1/4134cd82-6e48-4fc0-bcb4-1e3af369ef82.jpg?1572491298"),
                    LargeImage = new Uri("https://img.scryfall.com/cards/small/front/4/1/4134cd82-6e48-4fc0-bcb4-1e3af369ef82.jpg?1572491298"),
                    Name = "Island",
                    Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                },
                new FullCardModel()
                {
                    CardId = new Guid(),
                    SmallImage = new Uri("https://img.scryfall.com/cards/small/front/f/8/f8f03bb2-313e-4688-945f-052eed678174.jpg?1572491394"),
                    LargeImage = new Uri("https://img.scryfall.com/cards/small/front/f/8/f8f03bb2-313e-4688-945f-052eed678174.jpg?1572491394"),
                    Name = "Forest",
                    Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                },
            };

            this.SelectedCard = this.DeckCards[0];
        }
    }
}
