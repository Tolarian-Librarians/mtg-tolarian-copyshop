using System;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using Tolarian.Copyshop.Fontend.WPF.Communication;

namespace Tolarian.Copyshop.Fontend.WPF.Helper
{
    public class TolorianDeckPrinterHelper
    {
        private static XpsDocumentWriter _documentWriter;
        private readonly IDocumentPaginatorSource _document;
        private readonly Dialogs _dialogs;

        public TolorianDeckPrinterHelper(IDocumentPaginatorSource document, Dialogs dialogs)
        {
            this._document = document;
            this._dialogs = dialogs;
        }

        public void Print()
        {
            System.Printing.PrintDocumentImageableArea ia = null;

            // Only one printing job is allowed.
            if (_documentWriter != null)
            {
                return;
            }

            if (this._document != null)
            {
                // Show print dialog.
                XpsDocumentWriter docWriter = System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);
                if (docWriter != null && ia != null)
                {
                    // Register for WritingCompleted event.
                    _documentWriter = docWriter;
                    _documentWriter.WritingCompleted += this.HandlePrintCompleted;
                    _documentWriter.WritingCancelled += this.HandlePrintCancelled;

                    // Start Progress
                    this._dialogs.StartProgress("PRINT DECK",
                        "Please wait while your deck is printed..." + Environment.NewLine + Environment.NewLine +
                        "You should consider buying your Deck if you enjoy playing with it. Buying original cards supports the creation of new cards!");

                    // Write to the PrintQueue
                    if (this._document is FixedDocumentSequence)
                    {
                        docWriter.WriteAsync(this._document as FixedDocumentSequence);
                    }
                    else if (this._document is FixedDocument)
                    {
                        docWriter.WriteAsync(this._document as FixedDocument);
                    }
                    else
                    {
                        docWriter.WriteAsync(this._document.DocumentPaginator);
                    }
                }
            }
        }

        private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e) => this.CleanUpPrintOperation();
        private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e) => this.CleanUpPrintOperation();

        private void CleanUpPrintOperation()
        {
            if (_documentWriter != null)
            {
                _documentWriter.WritingCompleted -= this.HandlePrintCompleted;
                _documentWriter.WritingCancelled -= this.HandlePrintCancelled;
                _documentWriter = null;

                this._dialogs.EndProgress();
            }
        }
    }
}
