using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Mappings
{
    public class CreateWorkflowDecisionMapping : Profile
    {
        public CreateWorkflowDecisionMapping()
        {
            CreateMap<CreateWorkflowDecisionRequest, CreateWorkflowDecisionCommand>();
        }
    }
}
