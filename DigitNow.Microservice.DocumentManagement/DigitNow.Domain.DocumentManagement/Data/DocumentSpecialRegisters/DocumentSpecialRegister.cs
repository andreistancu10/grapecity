using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.SpecialRegisters;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters;

public class DocumentSpecialRegister : Entity
{
    public IncomingDocument Document { get; set; }
    public SpecialRegister SpecialRegister { get; set; }
    public long AssociationId { get; set; }
}