using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;


namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class GeneralObjectiveViewModel
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public BasicViewModel CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public BasicViewModel ModifiedBy { get; set; }
        public string Code { get; set; }
        public ObjectiveState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<DocumentFileMappingModel> ObjectiveUploadedFiles { get; set; }
    }
}
