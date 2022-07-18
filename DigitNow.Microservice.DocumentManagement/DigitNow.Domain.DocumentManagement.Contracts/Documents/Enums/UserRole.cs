
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public static List<UserRole> ListOfRoles { get; set; } = new List<UserRole> { HeadOfDepartment, Functionary, Mayor };

        public static UserRole HeadOfDepartment => new() { Code = "headOfDepartment", Id = 1 };
        public static UserRole Functionary => new() { Code = "functionary", Id = 2 };
        public static UserRole Mayor => new() { Code = "mayor", Id = 3 };
        public static UserRole Dashboard => new() { Code = "", Id = 4 };
    }
}
