using System;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.SpecialRegisters;

public class SpecialRegister : Entity
{
    public string Name { get; set; }
    public string Observations { get; set; }
    public int DocumentType { get; set; }
    public DateTime CreationDate { get; set; }
}