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
        public void PrintDeck(List<Uri> deckCards)
        {
            //FlowDocument doc = new FlowDocument(new Block());
            //doc.Name = "FlowDoc";

            //IDocumentPaginatorSource idpSource = doc;
            //printDlg.PrintDocument(idpSource.DocumentPaginator, "Hello WPF Printing.");

            BitmapImage bi = new BitmapImage(deckCards[0]);

            var vis = new DrawingVisual();
            using (var dc = vis.RenderOpen())
            {
                dc.DrawImage(bi, new Rect { Width = bi.Width * 200, Height = bi.Height * 250 });
            }

            PrintDialog printDlg = new PrintDialog();
            //if (printDlg.ShowDialog() == true)
            //{
                printDlg.PrintVisual(vis, "Image printing.");
            //}
        }
    }
}
