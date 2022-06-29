using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Validators;

public class SetDocumentsResolutionRequestValidator : AbstractValidator<SetDocumentsResolutionRequest>
{
    public SetDocumentsResolutionRequestValidator()
    {
        RuleFor(item => item.Batch).NotNull().NotEmpty();
        RuleFor(item => item.Batch.Documents).NotNull().NotEmpty();
        RuleFor(item => item.Resolution).NotEmpty();
    }
}