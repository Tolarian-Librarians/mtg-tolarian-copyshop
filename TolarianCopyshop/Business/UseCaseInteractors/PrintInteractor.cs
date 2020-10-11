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

        private Size _pageSize;
        private Thickness _pageMargin;
        private double _customCardWidth;
        private double _customCardHeight;

        public FixedDocument GetPrintPages(Size pageSize, Stack<Uri> deckCards, float scale)
        {
            FixedDocument doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = pageSize;
            this._pageSize = pageSize;

            if (deckCards is null || deckCards.Count == 0)
            {
                doc.Pages.Add(new PageContent());
                return doc;
            }

            this.SetScale(scale);
            this.SetPageMargin();
            this.AddCardImagesToDoc(deckCards, doc);
            return doc;
        }

        private void SetPageMargin()
        {
            // 3 cards seperated by 1 px
            double totalCardWidth = (this._customCardWidth * 3) + 2;
            double totalCardHeight = (this._customCardHeight * 3) + 2;

            double horizontalMargin = this._pageSize.Width - totalCardWidth;
            double vertialMargin = this._pageSize.Height - totalCardHeight;

            this._pageMargin = new Thickness(
                horizontalMargin / 2,
                vertialMargin / 2,
                horizontalMargin / 2,
                vertialMargin / 2);
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
                FixedPage page = GetPage(doc, this._pageMargin);

                this.AddCardImagesToPage(deckCards, page);

                ((IAddChild)pageContent).AddChild(page);
                doc.Pages.Add(pageContent);
            }
        }
        private static FixedPage GetPage(FixedDocument doc, Thickness pageMargin)
        {
            FixedPage page = new FixedPage
            {
                Width = doc.DocumentPaginator.PageSize.Width,
                Height = doc.DocumentPaginator.PageSize.Height,
                Margin = pageMargin,
            };
            return page;
        }

        private void AddCardImagesToPage(Stack<Uri> deckCards, FixedPage page)
        {
            ImageSource imgSource = null;
            Uri lastUri = null;

            for (int xPos = 0; xPos < _cardsPerRowAndCol; xPos++)
            {
                for (int yPos = 0; yPos < _cardsPerRowAndCol; yPos++)
                {
                    if (IsEveryCardAdded(deckCards))
                    {
                        return;
                    }

                    AddImageToPage(xPos, yPos);
                }
            }

            void AddImageToPage(int xPos, int yPos)
            {
                Uri uri = deckCards.Pop();
                if (lastUri != uri)
                {
                    imgSource = this.GetImageSourceFromUri(uri);
                }
                Image img = this.GetImageFromUri(imgSource, xPos, yPos);
                lastUri = uri;
                page.Children.Add(img);
            }
        }

        private static bool IsEveryCardAdded(Stack<Uri> deckCards)
            => deckCards.Count == 0;

        private Image GetImageFromUri(ImageSource source, int xPos, int yPos)
            => new Image
            {
                Source = source,
                Width = this._customCardWidth,
                Height = this._customCardHeight,
                RenderTransform = new TranslateTransform((this._customCardWidth * xPos) + xPos, (this._customCardHeight * yPos) + yPos),
            };

        private ImageSource GetImageSourceFromUri(Uri source)
            => new ImageSourceConverter().ConvertFromString(source.AbsoluteUri) as ImageSource;
    }
}