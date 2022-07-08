namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisterById;

public class GetSpecialRegisterByIdResponse
{
    public int Id { get; set; }
    public string Name{ get; set; }
    public int DocumentCategoryId { get; set; }
    public string Observations{ get; set; }
    public string CreatedAt { get; set; }
}