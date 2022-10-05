using DigitNow.Domain.DocumentManagement.Public.Procedures.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Procedures.Validators
{
    public class CreateProcedureValidator : AbstractValidator<CreateProcedureRequest>
    {
        public CreateProcedureValidator()
        {
            RuleFor(item => item.GeneralObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.SpecificObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.ActivityId).NotNull().NotEmpty();
            RuleFor(item => item.ProcedureCategory).NotNull().NotEmpty();
            RuleFor(item => item.DepartmentId).NotNull().NotEmpty();
            RuleFor(item => item.Title).NotNull().NotEmpty();
            RuleFor(item => item.Edition).NotNull().NotEmpty();
            RuleFor(item => item.StartDate).NotNull().NotEmpty();
            RuleFor(item => item.Scope).NotNull().NotEmpty();
            RuleFor(item => item.DomainOfApplicability).NotNull().NotEmpty();
            RuleFor(item => item.ProcedureDescription).NotNull().NotEmpty();
            RuleFor(item => item.Responsibility).NotNull().NotEmpty();
        }
    }
}
