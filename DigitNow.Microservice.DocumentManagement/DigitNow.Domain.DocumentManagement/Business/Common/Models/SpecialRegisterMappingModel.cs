using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models;

public class SpecialRegisterMappingModel
{
    public int DocumentId { get; set; }
    public long SpecialRegisterId { get; set; }

    #region [ References ]
    public Document Document { get; set; }
    public SpecialRegister SpecialRegister { get; set; }
    #endregion
}