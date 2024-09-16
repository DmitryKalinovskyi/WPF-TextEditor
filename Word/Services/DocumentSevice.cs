using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Word.Services
{
    public class DocumentSevice
    {
        public void SaveDocument(TextRange textRange)
        {
            var dialog = new SaveFileDialog();
            // set a default file name
            dialog.FileName = "document.rtf";
            // set filters - this can be done in properties as well

            dialog.Filter = "Document files (*.rtf)|*.rtf";
            dialog.ShowDialog();

            using var file = new FileStream(dialog.FileName, FileMode.Create);
            textRange.Save(file, DataFormats.Rtf);
            file.Close();
        }

        public void OpenDocument(TextRange textRange)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Document files (*.rtf)|*.rtf";
            var result = dialog.ShowDialog();

            if(result is null || result.Value == false)
            {
                // user decided to close dialog
                return;
            }

            using var file = new FileStream(dialog.FileName, FileMode.Open);

            textRange.Load(file, DataFormats.Rtf);
        }
    }
}
