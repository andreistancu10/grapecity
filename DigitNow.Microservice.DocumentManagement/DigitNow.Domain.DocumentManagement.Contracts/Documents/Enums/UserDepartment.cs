namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public class UserDepartment
    {
            public int Id { get; set; }
            public string Code { get; set; }

            public static UserDepartment MayorDepartment => new() { Code = "cabinetPrimar", Id = 4 };
            public static UserDepartment Registry => new() { Code = "registratura", Id = 1 };
            public static UserDepartment PublicAcquisition => new() { Code = "achizitiiPublice", Id = 7 };
    }
}
