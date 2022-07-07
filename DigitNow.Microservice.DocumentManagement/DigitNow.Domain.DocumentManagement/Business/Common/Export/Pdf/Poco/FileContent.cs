namespace DigitNow.Domain.DocumentManagement.Contracts.Documents
{
    public class FileContent
    {
        public string Name
        {
            get;
        }

        public string ContentType
        {
            get;
        }

        public byte[] Content
        {
            get;
        }

        public FileContent(string name, string contentType, byte[] content)
        {
            Name = name;
            ContentType = contentType;
            Content = content;
        }
    }
}
