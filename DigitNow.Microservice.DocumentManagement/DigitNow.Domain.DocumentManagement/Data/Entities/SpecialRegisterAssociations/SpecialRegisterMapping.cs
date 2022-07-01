using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterAssociations;

public class SpecialRegisterMapping : ExtendedEntity
{
    public int DocumentId { get; set; }
    public long SpecialRegisterId { get; set; }

    #region [ References ]
    public IncomingDocument IncomingDocument { get; set; }
    public SpecialRegister SpecialRegister { get; set; }
    #endregion
}