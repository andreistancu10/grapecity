using DigitNow.Domain.DocumentManagement.Public.Procedures.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Procedures.Validators
{
    public class UpdateProcedureValidator : AbstractValidator<UpdateProcedureRequest>
    {
        public UpdateProcedureValidator()
        {
            RuleFor(item => item.Id).NotNull().NotEmpty();
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
