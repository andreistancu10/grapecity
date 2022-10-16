namespace DigitNow.Domain.DocumentManagement.Public.Standards.Models
{
    public class CreateStandardRequest
    {
        public string Title { get; set; }
        public string Activity { get; set; }
        public long DepartmentId { get; set; }
        public DateTime Deadline { get; set; }
        public string Observations { get; set; }
        public List<long> StandardFunctionariesIds { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
