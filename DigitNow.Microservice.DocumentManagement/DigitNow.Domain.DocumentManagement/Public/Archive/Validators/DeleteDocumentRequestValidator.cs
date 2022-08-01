using DigitNow.Domain.DocumentManagement.Public.Archive.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Validators
{
    public class DeleteDocumentRequestValidator : AbstractValidator<DeleteDocumentRequest>
    {
        public DeleteDocumentRequestValidator()
        {
            RuleFor(item => item.DocumentId).NotNull().NotEmpty().WithMessage("DocumentId not specified!");
        }
    }
}
