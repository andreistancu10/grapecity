using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class GetActivityViewModel
    {
        public long Id { get; set; }
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public GeneralObjectiveDto AssociatedGeneralObjective { get; set; }
        public SpecificObjectiveDto AssociatedSpecificObjective { get; set; }
        public List<long> FunctionaryIds { get; set; }
    }
}
