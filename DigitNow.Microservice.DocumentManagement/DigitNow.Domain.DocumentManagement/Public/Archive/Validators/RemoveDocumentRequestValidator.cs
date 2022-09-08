using DigitNow.Domain.DocumentManagement.Public.Archive.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Validators
{
    public class RemoveDocumentRequestValidator : AbstractValidator<RemoveDocumentRequest>
    {
        public RemoveDocumentRequestValidator()
        {
            RuleFor(item => item.DocumentId).NotNull().NotEmpty().WithMessage("DocumentId not specified!");
        }
    }
}
