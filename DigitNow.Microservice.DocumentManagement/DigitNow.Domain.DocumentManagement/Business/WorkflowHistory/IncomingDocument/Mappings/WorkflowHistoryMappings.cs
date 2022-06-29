
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Commands.Create;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Mappings
{
    public class WorkflowHistoryMappings : Profile
    {
        public WorkflowHistoryMappings()
        {
            CreateMap<CreateWorkflowDecisionCommand, Data.Entities.WorkflowHistory>();
        }
    }
}
