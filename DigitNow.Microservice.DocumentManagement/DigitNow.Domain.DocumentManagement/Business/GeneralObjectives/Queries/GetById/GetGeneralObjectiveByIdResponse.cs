using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdResponse
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public string Code { get; set; }
        public ObjectiveState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }

        public List<ObjectiveUploadedFileDto> ObjectiveUploadedFiles { get; set; }
    }
}
