using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tolarian.Copyshop.Business
{
    public class PrintInteractor : IPrintRequester
    {
        const double _cardWidth = 226.7712;
        const double _cardHeight = 321.26016;
        const int _cardsPerRowAndCol = 3;

        public void PrintDeck(PrintDialog printDlg, Stack<Uri> deckCards)
        {
            FixedDocument doc = GetDocByDialogueSettings(printDlg);

            AddCardImagesToDoc(deckCards, doc);

            printDlg.PrintDocument(doc.DocumentPaginator, "Job name");
        }
        private static FixedDocument GetDocByDialogueSettings(PrintDialog printDlg)
        {
            FixedDocument doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
            return doc;
        }

        private void AddCardImagesToDoc(Stack<Uri> deckCards, FixedDocument doc)
        {
            while (deckCards.Count > 0)
            {
                PageContent pageContent = new PageContent();
                FixedPage page = GetPage(doc);

                AddCardImagesToPage(deckCards, page);

                ((IAddChild)pageContent).AddChild(page);
                doc.Pages.Add(pageContent);
            }
        }
        private static FixedPage GetPage(FixedDocument doc)
        {
            FixedPage page = new FixedPage();
            page.Width = doc.DocumentPaginator.PageSize.Width;
            page.Height = doc.DocumentPaginator.PageSize.Height;
            page.Background = Brushes.Black;
            return page;
        }

        private void AddCardImagesToPage(Stack<Uri> deckCards, FixedPage page)
        {
            for (int xPos = 0; xPos < _cardsPerRowAndCol; xPos++)
            {
                for (int yPos = 0; yPos < _cardsPerRowAndCol; yPos++)
                {
                    if (deckCards.Count == 0)
                        break;

                    Image img = GetImageForSlotFromUri(deckCards.Pop(), xPos, yPos);
                    page.Children.Add(img);
                }
            }
        }

        private Image GetImageForSlotFromUri(Uri source, int xPos, int yPos)
        {
            BitmapImage bitmap = new BitmapImage(source);

            var img = new Image { Source = bitmap };
            img.RenderSize = new Size(_cardWidth, _cardHeight);
            img.RenderTransform = new TranslateTransform(_cardWidth * xPos, _cardHeight * yPos);
            img.Width = _cardWidth;
            return img;
        }

    }
}
