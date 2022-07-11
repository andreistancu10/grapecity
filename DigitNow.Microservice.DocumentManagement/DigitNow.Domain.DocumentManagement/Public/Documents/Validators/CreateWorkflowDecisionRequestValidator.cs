using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Validators
{
    public class CreateWorkflowDecisionRequestValidator : AbstractValidator<CreateWorkflowDecisionRequest>
    {
        public CreateWorkflowDecisionRequestValidator()
        {
            RuleFor(item => item.DocumentType).NotNull().NotEmpty();
            RuleFor(item => item.InitiatorType).NotNull().NotEmpty();
            RuleFor(item => item.ActionType).NotNull().NotEmpty();
        }
    }
}
