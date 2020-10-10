using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
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

        public FixedDocument GetPrintPages(Size pageSize, Stack<Uri> deckCards, float scale)
        {
            FixedDocument doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = pageSize;

            if (deckCards is null || deckCards.Count == 0)
            {
                doc.Pages.Add(new PageContent());
                return doc;
            }

            this.SetScale(scale);
            this.AddCardImagesToDoc(deckCards, doc);
            return doc;
        }

        private void SetScale(float scale)
        {
            this._customCardWidth = _defaultCardWidth * scale;
            this._customCardHeight = _defaultCardHeight * scale;
        }

        private void AddCardImagesToDoc(Stack<Uri> deckCards, FixedDocument doc)
        {
            while (deckCards.Count > 0)
            {
                PageContent pageContent = new PageContent();
                FixedPage page = GetPage(doc);

                this.AddCardImagesToPage(deckCards, page);

                ((IAddChild)pageContent).AddChild(page);
                doc.Pages.Add(pageContent);
            }
        }
        private static FixedPage GetPage(FixedDocument doc)
        {
            FixedPage page = new FixedPage
            {
                Width = doc.DocumentPaginator.PageSize.Width,
                Height = doc.DocumentPaginator.PageSize.Height,
                Margin = new Thickness(_pageMargin)
            };
            return page;
        }

        private void AddCardImagesToPage(Stack<Uri> deckCards, FixedPage page)
        {
            for (int xPos = 0; xPos < _cardsPerRowAndCol; xPos++)
            {
                for (int yPos = 0; yPos < _cardsPerRowAndCol; yPos++)
                {
                    if (deckCards.Count == 0)
                    {
                        break;
                    }

                    UIElement img = this.GetImageForSlotFromUri(deckCards.Pop(), xPos, yPos);
                    page.Children.Add(img);
                }
            }
        }

        private UIElement GetImageForSlotFromUri(Uri source, int xPos, int yPos)
        {
            Image img = new Image
            {
                Source = new ImageSourceConverter().ConvertFromString(source.AbsoluteUri) as ImageSource,
                Width = this._customCardWidth,
                Height = this._customCardHeight,

                RenderTransform = new TranslateTransform((this._customCardWidth * xPos) + xPos, (this._customCardHeight * yPos) + yPos),
            };

            return img;
        }
    }
}