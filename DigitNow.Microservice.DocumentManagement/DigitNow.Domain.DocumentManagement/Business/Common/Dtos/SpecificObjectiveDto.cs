using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class SpecificObjectiveDto
    {
        public long ObjectiveId { get; set; }
        public string Code { get; set; }
        public long StateId{ get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public long DepartmentId { get; set; }
        public List<FunctionaryDto> SpecificObjectiveFunctionaries { get; set; }
    }
}
