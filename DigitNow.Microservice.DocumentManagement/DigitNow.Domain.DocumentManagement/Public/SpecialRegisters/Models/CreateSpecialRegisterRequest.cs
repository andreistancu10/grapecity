namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;

public class CreateSpecialRegisterRequest
{
    public int DocumentCategoryId { get; set; }
    public string Name { get; set; }
    public string Observations { get; set; }
}