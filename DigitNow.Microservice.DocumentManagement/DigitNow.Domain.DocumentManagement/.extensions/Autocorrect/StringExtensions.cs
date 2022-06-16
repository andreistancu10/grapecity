using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.extensions.Autocorrect
{
    public static class StringExtensions
    {
        public static string ToUpperFirstCharacter(this string source)
        {
            return source switch
            {
                null => throw new ArgumentNullException(nameof(source)),
                "" => source,
                _ => source[..1].ToUpper() + source[1..]
            };
        }
        
        public static string FormatUserNameByRole(this User user, UserRole role)
        {
            return role switch
            {
                UserRole.HeadOfDepartment => user.LastName + " " + user.FirstName + " Sef Departament",
                UserRole.Functionary => user.LastName + " " + user.FirstName + " Functionar",
                UserRole.Mayor => user.LastName + " " + user.FirstName + " Primar",
                _ => string.Empty,
            };
        }
    }
}