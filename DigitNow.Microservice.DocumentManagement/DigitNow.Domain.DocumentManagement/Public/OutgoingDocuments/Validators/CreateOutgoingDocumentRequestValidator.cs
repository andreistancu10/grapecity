using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Validators;

public class CreateOutgoingDocumentRequestValidator : AbstractValidator<CreateOutgoingDocumentRequest>
{
    public CreateOutgoingDocumentRequestValidator()
    {
        //RuleFor(item => item.InputChannelId).NotNull().NotEmpty();
        //RuleFor(item => item.IssuerTypeId).NotNull().NotEmpty();
        //RuleFor(item => item.IssuerName).NotNull().NotEmpty();
        //RuleFor(item => item.ExternalNumber).NotNull().NotEmpty();
        //RuleFor(item => item.ContentSummary).NotNull().NotEmpty();
        //RuleFor(item => item.NumberOfPages).NotNull().NotEmpty();
        //RuleFor(item => item.RecipientId).NotNull().NotEmpty();
        //RuleFor(item => item.DocumentTypeId).NotNull().NotEmpty();
        //RuleFor(item => item.ResolutionPeriod).NotNull().NotEmpty();
        //RuleFor(item => item.ContactDetail.CountryId).NotNull().NotEmpty();
        //RuleFor(item => item.ContactDetail.CountyId).NotNull().NotEmpty();
        //RuleFor(item => item.ContactDetail.CityId).NotNull().NotEmpty();
    }
}