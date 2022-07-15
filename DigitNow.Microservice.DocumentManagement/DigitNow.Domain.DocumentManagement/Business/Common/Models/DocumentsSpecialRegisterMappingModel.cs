using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models;

public class DocumentsSpecialRegisterMappingModel
{
    public long DocumentId { get; set; }
    public long SpecialRegisterId { get; set; }

    #region [ References ]

    public SpecialRegister SpecialRegister { get; set; }

    #endregion
}