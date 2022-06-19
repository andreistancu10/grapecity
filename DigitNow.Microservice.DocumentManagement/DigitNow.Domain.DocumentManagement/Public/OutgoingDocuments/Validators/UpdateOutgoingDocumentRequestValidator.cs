using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Validators;

public class UpdateOutgoingDocumentRequestValidator : AbstractValidator<UpdateOutgoingDocumentRequest>
{
    public UpdateOutgoingDocumentRequestValidator()
    {
        RuleFor(item => item.RecipientId).NotNull().NotEmpty();
        RuleFor(item => item.RecipientName).NotNull().NotEmpty();
        RuleFor(item => item.RecipientTypeId).NotNull().NotEmpty();
        RuleFor(item => item.ConnectedDocumentIds).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CountryId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CountyId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CityId).NotNull().NotEmpty();
        RuleFor(item => item.ContentSummary).NotNull().NotEmpty();
        RuleFor(item => item.DocumentTypeId).NotNull().NotEmpty();
        RuleFor(item => item.DocumentTypeDetail).NotNull().NotEmpty().When(item => item.DocumentTypeId == 10);
        RuleFor(item => item.NumberOfPages).NotNull().NotEmpty();
        RuleFor(item => item.User).NotNull().NotEmpty();
    }
}