using System.IO;
using Word.Models;

namespace Word.Services
{
    public class DocumentRepository
    {
        public void SaveDocument(DocumentModel documentModel)
        {
            if (documentModel.Path is null)
            {
                throw new ArgumentNullException(nameof(documentModel.Path), "Document path is required to save.");
            }

            using var file = new FileStream(documentModel.Path, FileMode.Create);
            using var streamWriter = new StreamWriter(file);
            streamWriter.Write(documentModel.Content);
        }

        public DocumentModel? OpenDocument(string path)
        {
            using var file = new FileStream(path, FileMode.Open);
            using var streamReader = new StreamReader(file);

            return new DocumentModel()
            {
                Path = path,
                Content = streamReader.ReadToEnd()
            };
        }
    }
}
