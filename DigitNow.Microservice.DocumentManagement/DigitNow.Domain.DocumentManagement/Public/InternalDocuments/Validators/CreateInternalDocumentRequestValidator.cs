using DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Validators;

public class CreateInternalDocumentRequestValidator : AbstractValidator<CreateInternalDocumentRequest>
{
    public CreateInternalDocumentRequestValidator()
    {
        RuleFor(item => item.SourceDepartmentId).NotNull().NotEmpty();
        RuleFor(item => item.InternalDocumentTypeId).NotNull().NotEmpty();
        RuleFor(item => item.DeadlineDaysNumber).NotNull().NotEmpty();
        RuleFor(item => item.Description).NotNull().NotEmpty();
        RuleFor(item => item.DestinationDepartmentId).NotNull().NotEmpty();
    }
}