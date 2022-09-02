using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class GetSpecificObjectiveViewModel
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public string Code { get; set; }
        public ScimState State { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public long DepartmentId { get; set; }
        public long GeneralObjectiveId { get; set; }
        public GeneralObjectiveDto AssociatedGeneralObjective { get; set; }
        public List<long> FunctionaryId { get; set; }
    }
}
