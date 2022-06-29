using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

public class SpecialRegister : IExtendedEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Observations { get; set; }
    public int DocumentType { get; set; }

    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}