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
            _document = document;
            _dialogs = dialogs;
        }

        public void Print()
        {
            System.Printing.PrintDocumentImageableArea ia = null;

            // Only one printing job is allowed.
            if (_documentWriter != null)
            {
                return;
            }

            if (_document != null)
            {
                // Show print dialog.
                XpsDocumentWriter docWriter = System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);
                if (docWriter != null && ia != null)
                {
                    // Register for WritingCompleted event.
                    _documentWriter = docWriter;
                    _documentWriter.WritingCompleted += HandlePrintCompleted;
                    _documentWriter.WritingCancelled += HandlePrintCancelled;

                    // Start Progress
                    _dialogs.StartProgress("PRINT DECK",
                        "Please wait while your deck is printed..." + Environment.NewLine + Environment.NewLine +
                        "You should consider buying your Deck if you enjoy playing with it. Buying original cards supports the creation of new cards!");

                    // Write to the PrintQueue
                    if (_document is FixedDocumentSequence)
                    {
                        docWriter.WriteAsync(_document as FixedDocumentSequence);
                    }
                    else if (_document is FixedDocument)
                    {
                        docWriter.WriteAsync(_document as FixedDocument);
                    }
                    else
                    {
                        docWriter.WriteAsync(_document.DocumentPaginator);
                    }
                }
            }
        }

        private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e) => CleanUpPrintOperation();
        private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e) => CleanUpPrintOperation();

        private void CleanUpPrintOperation()
        {
            if (_documentWriter != null)
            {
                _documentWriter.WritingCompleted -= HandlePrintCompleted;
                _documentWriter.WritingCancelled -= HandlePrintCancelled;
                _documentWriter = null;

                _dialogs.EndProgress();
            }
        }
    }
}