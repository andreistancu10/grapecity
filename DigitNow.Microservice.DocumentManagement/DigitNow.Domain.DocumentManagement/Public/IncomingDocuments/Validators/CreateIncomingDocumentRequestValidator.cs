using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Validators;

public class CreateIncomingDocumentRequestValidator : AbstractValidator<CreateIncomingDocumentRequest>
{
    public CreateIncomingDocumentRequestValidator()
    {
        RuleFor(item => item.InputChannelId).NotNull().NotEmpty();
        RuleFor(item => item.IssuerTypeId).NotNull().NotEmpty();
        RuleFor(item => item.IssuerName).NotNull().NotEmpty();
        RuleFor(item => item.ContentSummary).NotNull().NotEmpty();
        RuleFor(item => item.NumberOfPages).NotNull().NotEmpty();
        RuleFor(item => item.RecipientId).NotNull().NotEmpty();
        RuleFor(item => item.DocumentTypeId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CountryId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CountyId).NotNull().NotEmpty();
        RuleFor(item => item.ContactDetail.CityId).NotNull().NotEmpty();

        RuleFor(item => item.ExternalNumberDate).NotNull().NotEmpty().When(item => item.ExternalNumber > 0);
        RuleFor(item => item.IsGDPRAgreed).Equal(true).When(item => item.InputChannelId == (int)InputChannel.Window && item.IssuerTypeId == (int)IssuerType.Company);
        RuleFor(item => item.Detail).NotNull().NotEmpty().When(item => item.DocumentTypeId == 12);
    }
}