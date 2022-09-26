using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById
{
    public class GetActionByIdResponse
    {
        public long Id { get; set; }
        public ActivityDto Activity { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public ScimState StateId { get; set; }
        public string Details { get; set; }
        public string ModificationMotive { get; set; }
        public List<ActionFunctionaryResponse> ActionFunctionaries { get; set; }
    }

    public class ActionFunctionaryResponse
    {
        public long Id { get; set; }
        public long ActionId { get; set; }
        public long FunctionaryId { get; set; }
    }
}
