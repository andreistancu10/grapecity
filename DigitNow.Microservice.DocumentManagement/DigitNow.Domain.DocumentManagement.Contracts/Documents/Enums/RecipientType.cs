
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public class RecipientType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public static List<RecipientType> ListOfTypes { get; set; } = new List<RecipientType> { HeadOfDepartment, Functionary, Mayor };

        public static RecipientType HeadOfDepartment => new() { Code = "headOfDepartment", Id = 1 };
        public static RecipientType Functionary => new() { Code = "functionary", Id = 2 };
        public static RecipientType Mayor => new() { Code = "mayor", Id = 3 };
        public static RecipientType Department => new() { Code = "department", Id = 4 };
    }
}
