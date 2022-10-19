namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class StandardViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public BasicViewModel Department { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public long StateId { get; set; }
    }
}
