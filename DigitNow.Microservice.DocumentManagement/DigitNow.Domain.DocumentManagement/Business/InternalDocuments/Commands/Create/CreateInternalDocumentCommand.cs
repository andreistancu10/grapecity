using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocument.Commands.Create;

public class CreateInternalDocumentCommand : ICommand<ResultObject>
{
    public int DepartmentId { get; set; }
    public int InternalDocumentTypeId { get; set; }
    public int DeadlineDaysNumber { get; set; }
    public string Description { get; set; }
    public string Observation { get; set; }
    public int ReceiverDepartmentId { get; set; }
    public bool? IsUrgent { get; set; }
    public string User { get; set; }
    
    
}
