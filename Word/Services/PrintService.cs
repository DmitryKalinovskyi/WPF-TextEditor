using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Word.Models;

namespace Word.Services
{
    public class PrintService
    {
        public void Print(DocumentModel document)
        {
            var dialog = new PrintDialog();

            // Check if the user confirmed the print dialog
            if (dialog.ShowDialog() == true)
            {
                // Create a FlowDocument with the content of the document
                FlowDocument flowDocument = ConvertRtfToFlowDocument(document.Content);

                // Create a DocumentPaginator from the FlowDocument
                IDocumentPaginatorSource documentPaginator = flowDocument;

                // Print the document using the PrintDialog
                dialog.PrintDocument(documentPaginator.DocumentPaginator, "Print Document");
            }
        }

        public FlowDocument ConvertRtfToFlowDocument(string rtfContent)
        {
            // Create a new FlowDocument
            FlowDocument flowDocument = new FlowDocument();

            // Use a RichTextBox to load the RTF content
            RichTextBox richTextBox = new RichTextBox();

            // Load the RTF content into the RichTextBox
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(rtfContent));
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

            // Try to load the RTF content into the TextRange
            textRange.Load(memoryStream, DataFormats.Rtf);

            // Return the document from the RichTextBox
            flowDocument = richTextBox.Document;

            return flowDocument;
        }
    }
}
