using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tolarian.Copyshop.Business
{
    public class PrintInteractor : IPrintRequester
    {
        const double _cardWidth = 226.7712;
        const double _cardHeight = 321.26016;
        const int _cardsPerRowAndCol = 3;

        public void PrintDeck(List<Uri> deckCards)
        {
            FlowDocument doc = new FlowDocument();
            doc.Name = "FlowDoc";

            IDocumentPaginatorSource idpSource = doc;
            //printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");

            Rect[,] cardSlots = GetCardPositions();
            Stack<BitmapImage> stack = new Stack<BitmapImage>(deckCards.Select(u => new BitmapImage(u)));

            var vis = new DrawingVisual();
            using (var dc = vis.RenderOpen())
            {
                dc.DrawRectangle(Brushes.Black, null, GetA4Rect());

                for (int i = 0; i < _cardsPerRowAndCol; i++)
                {
                    for (int j = 0; j < _cardsPerRowAndCol; j++)
                    {
                        dc.DrawImage(stack.Pop(), cardSlots[i, j]);
                    }
                }
            }


            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintVisual(vis, "Image printing.");
            
        }

        private Rect GetA4Rect()
        {
            return new Rect { Width = 793.92, Height = 1122.24 };
        }

        private Rect GetCardRect()
        {
            return new Rect { Width = 226.7712, Height = 321.26016 };
        }

        private Rect[,] GetCardPositions()
        {
            int cardCount = _cardsPerRowAndCol; 

            Rect[,] result = new Rect[cardCount, cardCount];

            for (int i = 0; i < cardCount; i++)
            {
                for (int j = 0; j < cardCount; j++)
                {
                    result[i, j] = new Rect { Width = _cardWidth, Height = _cardHeight, Location = new Point { X = i * _cardWidth, Y = j * _cardHeight } };
                }
            }

            return result;
        }
    }
}
