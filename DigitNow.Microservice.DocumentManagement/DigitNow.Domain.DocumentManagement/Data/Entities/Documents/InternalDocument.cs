using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class InternalDocument : VirtualDocument
{
    public long DocumentId { get; set; }
    public int DepartmentId { get; set; }
    public int InternalDocumentTypeId { get; set; }
    public int DeadlineDaysNumber { get; set; }
    public string Description { get; set; }
    public string Observation { get; set; }
    public int ReceiverDepartmentId { get; set; }
    public bool? IsUrgent { get; set; }

    #region [ References ]

    public Document Document { get; set; }

    #endregion
}