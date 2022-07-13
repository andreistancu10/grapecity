
namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public static UserRole HeadOfDepartment => new UserRole { Code = "headOfDepartment", Id = 1 };
        public static UserRole Functionary => new UserRole { Code = "functionary", Id = 2 };
        public static UserRole Mayor => new UserRole { Code = "mayor", Id = 3 };
    }
}
