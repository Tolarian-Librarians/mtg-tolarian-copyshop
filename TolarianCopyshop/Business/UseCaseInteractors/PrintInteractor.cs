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
        private const double _defaultCardWidth = 226.7712;
        private const double _defaultCardHeight = 321.26016;

        private const int _cardsPerRowAndCol = 3;
        private const int _pageMargin = 40;

        private double _customCardWidth;
        private double _customCardHeight;


        public void PrintDeck(PrintDialog printDlg, Stack<Uri> deckCards, float scale)
        {
            if (deckCards is null || deckCards.Count == 0)
            {
                return;
            }

            _customCardWidth = _defaultCardWidth * scale;
            _customCardHeight = _defaultCardHeight * scale;

            FixedDocument doc = GetDocByDialogueSettings(printDlg);

            AddCardImagesToDoc(deckCards, doc);

            printDlg.PrintDocument(doc.DocumentPaginator, "MtgProxyDeck");
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
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = source;
            bitmap.EndInit();

            var img = new Image();
            img.Source = bitmap;
            img.Width = _customCardWidth;
            img.Height = _customCardHeight;

            img.RenderTransform = new TranslateTransform((_customCardWidth * xPos) + xPos, (_customCardHeight * yPos) + yPos);

            return img;
        }
    }
}