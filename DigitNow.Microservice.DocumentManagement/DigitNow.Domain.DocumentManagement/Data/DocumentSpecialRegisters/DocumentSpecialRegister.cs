using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.SpecialRegisters;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters;

public class DocumentSpecialRegister : Entity
{
    public int DocumentId { get; set; }
    public long SpecialRegisterId { get; set; }
    public long? AssociationId { get; set; }
    public virtual IncomingDocument Document { get; set; }
    public virtual SpecialRegister SpecialRegister { get; set; }
}