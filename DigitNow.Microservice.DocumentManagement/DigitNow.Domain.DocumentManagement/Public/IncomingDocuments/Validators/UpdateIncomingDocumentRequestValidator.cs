using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Validators
{

    public class UpdateIncomingDocumentRequestValidator : AbstractValidator<UpdateIncomingDocumentRequest>
    {
        public UpdateIncomingDocumentRequestValidator()
        {
            RuleFor(item => item.InputChannelId).NotNull().NotEmpty();
            RuleFor(item => item.IssuerTypeId).NotNull().NotEmpty();
            RuleFor(item => item.IssuerName).NotNull().NotEmpty();
            RuleFor(item => item.ExternalNumber).NotNull().NotEmpty();
            RuleFor(item => item.ContentSummary).NotNull().NotEmpty();
            RuleFor(item => item.NumberOfPages).NotNull().NotEmpty();
            RuleFor(item => item.RecipientId).NotNull().NotEmpty();
            RuleFor(item => item.DocumentTypeId).NotNull().NotEmpty();
            RuleFor(item => item.ResolutionPeriod).NotNull().NotEmpty();
            RuleFor(item => item.ContactDetail.Id).NotNull().NotEmpty();
            RuleFor(item => item.ContactDetail.CountryId).NotNull().NotEmpty();
            RuleFor(item => item.ContactDetail.CountyId).NotNull().NotEmpty();
            RuleFor(item => item.ContactDetail.CityId).NotNull().NotEmpty();
        }
    }
}