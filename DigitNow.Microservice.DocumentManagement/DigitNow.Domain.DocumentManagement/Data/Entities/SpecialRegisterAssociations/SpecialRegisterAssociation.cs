using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterAssociations;

public class SpecialRegisterAssociation : ExtendedEntity
{
    public int DocumentId { get; set; }
    public long SpecialRegisterId { get; set; }
    public IncomingDocument Document { get; set; }
    public SpecialRegister SpecialRegister { get; set; }
}