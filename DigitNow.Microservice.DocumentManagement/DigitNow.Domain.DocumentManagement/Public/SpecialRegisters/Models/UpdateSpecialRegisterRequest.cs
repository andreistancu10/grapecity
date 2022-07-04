namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;

public class UpdateSpecialRegisterRequest
{
    public int DocumentCategoryId { get; set; }
    public string Observations { get; set; }
}