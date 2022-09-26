using DigitNow.Domain.DocumentManagement.Public.Risks.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Risks.Validators
{
    public class CreateRiskTrackingReportValidator : AbstractValidator<CreateRiskTrackingReportRequest>
    {
        public CreateRiskTrackingReportValidator()
        {
            RuleFor(item => item.ControlMeasuresImplementationState).NotNull().NotEmpty();
            RuleFor(item => item.ProbabilityOfApparitionEstimation).NotNull().NotEmpty();
            RuleFor(item => item.ImpactOfObjectivesEstimation).NotNull().NotEmpty();
            RuleFor(item => item.RiskExposureEvaluation).NotNull().NotEmpty();

            RuleForEach(item => item.RiskActionProposals).ChildRules(action =>
            {
                action.RuleFor(x => x.ProposedAction).NotEmpty();
                action.RuleFor(x => x.Deadline).NotEmpty();
                action.RuleFor(x => x.RiskTrackingReportDate).NotEmpty();
            });
        }
    }
}
