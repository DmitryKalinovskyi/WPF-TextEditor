namespace Word.Models
{
    public class DocumentModel
    {
        public string? Path { get; set; }

        public string Content { get; set; }

        public DocumentModel()
        {
            Content = string.Empty;
        }
    }
}
