using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class ActivityDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public long StateId{ get; set; }
        public string Details { get; set; }
        public long DepartmentId { get; set; }
        public List<FunctionaryDto> ActivityFunctionaries { get; set; }
    }
}