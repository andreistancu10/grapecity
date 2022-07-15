using DigitNow.Domain.DocumentManagement.Public.Archive.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Archive.Validators;

public class ArchiveValidator : AbstractValidator<GetArchivedDocumentsRequest>
{
    public ArchiveValidator()
    {
    }
}