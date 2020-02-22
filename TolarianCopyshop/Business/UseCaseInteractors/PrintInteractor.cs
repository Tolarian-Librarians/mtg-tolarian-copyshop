using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tolarian.Copyshop.Business.Interfaces;

namespace Tolarian.Copyshop.Business.UseCaseInteractors
{
    public class PrintInteractor : IPrintRequester
    {
        const double _cardWidth = 226.7712;
        const double _cardHeight = 321.26016;
        const int _cardsPerRowAndCol = 3;
        private const int _pageMargin = 50;

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
            page.Margin = new Thickness(_pageMargin);
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

                    UIElement img = GetImageForSlotFromUri(deckCards.Pop(), xPos, yPos);
                    page.Children.Add(img);
                }
            }
        }

        private UIElement GetImageForSlotFromUri(Uri source, int xPos, int yPos)
        {
            const int cropLeft = 5;
            const int cropRight = 5;
            const int cropTop = 5;
            const int cropBottom = 8;

            //Those need to be added to the images width and height to correct its size after clipping (which made it too small at first)
            int offsetX = cropLeft + cropRight;
            int offsetY = cropTop + cropBottom;

            BitmapImage bitmap = new BitmapImage(source);

            var img = new Image();
            img.Source = bitmap;
            img.Width = _cardWidth + offsetX;
            img.Height = _cardHeight + offsetY;
            img.Clip = new RectangleGeometry(new Rect(cropLeft, cropTop, img.Width - (cropRight + cropLeft), img.Height - (cropBottom + cropTop)));

            img.RenderTransform = new TranslateTransform(_cardWidth * xPos, _cardHeight * yPos);

            return img;
        }
    }
}