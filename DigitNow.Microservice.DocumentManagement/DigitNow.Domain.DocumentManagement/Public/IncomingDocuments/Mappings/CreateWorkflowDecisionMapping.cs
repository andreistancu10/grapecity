using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Mappings
{
    public class CreateWorkflowDecisionMapping : Profile
    {
        public CreateWorkflowDecisionMapping()
        {
            CreateMap<CreateWorkflowDecisionRequest, CreateWorkflowDecisionCommand>();
        }
    }
}
