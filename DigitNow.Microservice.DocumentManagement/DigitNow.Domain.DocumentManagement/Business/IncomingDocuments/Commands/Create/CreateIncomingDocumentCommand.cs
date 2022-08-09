using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;

public class CreateIncomingDocumentCommand : ICommand<ResultObject>
{
    public DateTime? RegistrationDate { get; set; }
    public int InputChannelId { get; set; }
    public int IssuerTypeId { get; set; }
    public string IssuerName { get; set; }
    public string IdentificationNumber { get; set; }
    public int ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public ContactDetailDto ContactDetail { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public string Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool? IsUrgent { get; set; }
    public bool? IsGDPRAgreed { get; set; }
    public List<long> ConnectedDocumentIds { get; set; }
    public List<long> UploadedFileIds { get; set; }
}