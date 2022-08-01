using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;

public class CreateOutgoingDocumentCommand : ICommand<ResultObject>
{
    public int DestinationDepartmentId { get; set; }
    public string RecipientName { get; set; }
    public string? IdentificationNumber { get; set; }
    public ContactDetailDto ContactDetail { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int DocumentTypeId { get; set; }
    public string? DocumentTypeDetail { get; set; }
    public List<long>? ConnectedDocumentIds { get; set; }
    public List<long> UploadedFileIds { get; set; }
}