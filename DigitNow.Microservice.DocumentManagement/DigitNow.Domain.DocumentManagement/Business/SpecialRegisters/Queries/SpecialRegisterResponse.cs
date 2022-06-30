using System;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;

public class SpecialRegisterResponse
{
    public int Id { get; set; }
    public string Name{ get; set; }
    public DateTime CreationDate { get; set; }
}