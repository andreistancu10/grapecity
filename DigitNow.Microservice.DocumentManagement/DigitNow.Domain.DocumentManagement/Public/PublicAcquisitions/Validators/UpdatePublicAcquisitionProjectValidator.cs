using DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Validators
{
    public class UpdatePublicAcquisitionProjectValidator : AbstractValidator<UpdatePublicAcquisitionProjectRequest>
    {
        public UpdatePublicAcquisitionProjectValidator()
        {
            RuleFor(item => item.ProjectYear).NotNull().NotEmpty();
            RuleFor(item => item.Type).NotNull().NotEmpty();
            RuleFor(item => item.CpvCode).NotNull().NotEmpty();
            RuleFor(item => item.EstimatedValue).NotNull().NotEmpty();
            RuleFor(item => item.FinancingSource).NotNull().NotEmpty();
            RuleFor(item => item.EstablishedProcedure).NotNull().NotEmpty();
            RuleFor(item => item.EstimatedMonthForInitiatingProcedure).NotNull().NotEmpty();
            RuleFor(item => item.EstimatedMonthForProcedureAssignment).NotNull().NotEmpty();
            RuleFor(item => item.ProcedureAssignmentMethod).NotNull().NotEmpty();
            RuleFor(item => item.ProcedureAssignmentResponsible).NotNull().NotEmpty();
        }
    }
}
