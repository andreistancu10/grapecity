using System.Collections.Generic;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;

public class CreateInternalDocumentCommand : ICommand<ResultObject>
{
    public int SourceDepartmentId { get; set; }
    public int InternalDocumentTypeId { get; set; }
    public int DeadlineDaysNumber { get; set; }
    public string Description { get; set; }
    public string Observation { get; set; }
    public int DestinationDepartmentId { get; set; }
    public bool? IsUrgent { get; set; }
    public List<long> UploadedFileIds { get; set; }
}