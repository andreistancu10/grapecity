using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Validators
{
    public class UpdateDocumentRecipientRequestValidator : AbstractValidator<UpdateDocumentRecipientRequest>
    {
        public UpdateDocumentRecipientRequestValidator()
        {
            RuleFor(item => item.DepartmentId).NotNull().NotEmpty().WithMessage("Deptartment not specified!");

            RuleForEach(request => request.DocumentInfo).ChildRules(docInfo =>
            {
                docInfo.RuleFor(doc => doc.RegistrationNumber).GreaterThan(0).WithMessage("Registration number is invalid");
                docInfo.RuleFor(doc => doc.DocType).GreaterThan(0).WithMessage("Document Type not specified!");
            });
        }
    }
}
