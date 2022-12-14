using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Validators;

public class CreateOutgoingDocumentRequestValidator : AbstractValidator<CreateOutgoingDocumentRequest>
{
    public CreateOutgoingDocumentRequestValidator()
    {
        RuleFor(item => item.DestinationDepartmentId).NotNull().NotEmpty();
        RuleFor(item => item.RecipientName).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CountryId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CountyId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CityId).NotNull().NotEmpty();
        RuleFor(item => item.ContentSummary).NotNull().NotEmpty();
        RuleFor(item => item.DocumentTypeId).NotNull().NotEmpty();
        RuleFor(item => item.NumberOfPages).NotNull().NotEmpty();
    }
}