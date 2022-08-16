namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models
{
    public class CreateGeneralObjectiveRequest
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
