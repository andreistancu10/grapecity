using DigitNow.Domain.DocumentManagement.Public.Risks.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Risks.Validators
{
    public class CreateRiskValidator : AbstractValidator<CreateRiskRequest>
    {
        public CreateRiskValidator()
        {
            RuleFor(item => item.GeneralObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.SpecificObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.ActivityId).NotNull().NotEmpty();
            RuleFor(item => item.DepartmentId).NotNull().NotEmpty();
            RuleFor(item => item.Description).NotNull().NotEmpty();
            RuleFor(item => item.RiskCauses).NotNull().NotEmpty();
            RuleFor(item => item.RiskConsequences).NotNull().NotEmpty();
            RuleFor(item => item.ProbabilityOfApparitionEstimation).NotNull().NotEmpty();
            RuleFor(item => item.ImpactOfObjectivesEstimation).NotNull().NotEmpty();
            RuleFor(item => item.HeadOfDepartmentDecision).NotNull().NotEmpty();
            RuleFor(item => item.AdoptedStrategy).NotNull().NotEmpty();

            RuleForEach(item => item.RiskControlActions).ChildRules(action =>
            {
                action.RuleFor(x => x.ControlMeasurement).NotEmpty();
                action.RuleFor(x => x.Deadline).NotEmpty();
            });
        }
    }
}
