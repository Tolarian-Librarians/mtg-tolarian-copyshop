using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tolarian.Copyshop.ScreenPresenter.ViewModels;

namespace Tolarian.Copyshop.ScreenPresenter.Views
{
    /// <summary>
    /// Interaktionslogik für DeckBuilder.xaml
    /// </summary>
    public partial class DeckBuilderView : UserControl
    {
        public DeckBuilderView()
        {
            InitializeComponent();
            this.DataContext = new DeckBuilderViewModel(null)
            {
                Cards = new System.Collections.ObjectModel.ObservableCollection<Models.Card>
                {
                    new Models.Card()
                    {
                        ID = new Guid(),
                        Picture = new BitmapImage(new Uri("https://img.scryfall.com/cards/small/front/3/2/32982ed2-96e4-4cc5-8562-744b06bca239.jpg?1572491355")),
                        PictureLarge = new BitmapImage(new Uri("https://img.scryfall.com/cards/large/front/3/2/32982ed2-96e4-4cc5-8562-744b06bca239.jpg?1572491355")),
                        Name = "Mountain",
                        Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                    },
                    new Models.Card()
                    {
                        ID = new Guid(),
                        Picture = new BitmapImage(new Uri("https://img.scryfall.com/cards/small/front/e/4/e4f184c5-4f3c-4aea-afa1-f0903d3cc71a.jpg?1572491327")),
                        Name = "Swamp",
                        Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                    },
                    new Models.Card()
                    {
                        ID = new Guid(),
                        Picture = new BitmapImage(new Uri("https://img.scryfall.com/cards/small/front/7/d/7ded4b2a-ba56-43da-8ea7-392b77fc2926.jpg?1572491259")),
                        Name = "Plains",
                        Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                    },
                    new Models.Card()
                    {
                        ID = new Guid(),
                        Picture = new BitmapImage(new Uri("https://img.scryfall.com/cards/small/front/4/1/4134cd82-6e48-4fc0-bcb4-1e3af369ef82.jpg?1572491298")),
                        Name = "Island",
                        Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                    },
                    new Models.Card()
                    {
                        ID = new Guid(),
                        Picture = new BitmapImage(new Uri("https://img.scryfall.com/cards/small/front/f/8/f8f03bb2-313e-4688-945f-052eed678174.jpg?1572491394")),
                        Name = "Forest",
                        Text="Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet."
                    },
                }
            };
        }
    }
}
