namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class DocumentFileViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime UploadDate { get; set; }
        public BasicViewModel Category { get; set; }
        public BasicViewModel UploadedBy { get; set; }
    }
}