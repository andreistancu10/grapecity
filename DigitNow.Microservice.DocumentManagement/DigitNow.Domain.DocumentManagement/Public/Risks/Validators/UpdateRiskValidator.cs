using DigitNow.Domain.DocumentManagement.Public.Risks.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Risks.Validators
{
    public class UpdateRiskValidator : AbstractValidator<UpdateRiskRequest>
    {
        public UpdateRiskValidator()
        {
            RuleFor(item => item.Description).NotNull().NotEmpty();
            RuleFor(item => item.RiskCauses).NotNull().NotEmpty();
            RuleFor(item => item.RiskConsequences).NotNull().NotEmpty();
            RuleFor(item => item.ProbabilityOfApparitionEstimation).NotNull().NotEmpty();
            RuleFor(item => item.ImpactOfObjectivesEstimation).NotNull().NotEmpty();
            RuleFor(item => item.HeadOfDepartmentDecision).NotNull().NotEmpty();
            RuleFor(item => item.AdoptedStrategy).NotNull().NotEmpty();
        }
    }
}
