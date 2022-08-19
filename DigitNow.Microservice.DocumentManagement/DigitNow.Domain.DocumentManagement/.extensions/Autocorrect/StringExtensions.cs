using DigitNow.Domain.DocumentManagement.Business.Common.Models;
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
        
        public static string FormatUserNameByRole(this UserModel user, string role)
        {
            switch (role)
            {
                case "headOfDepartment":
                    return user.LastName + " " + user.FirstName + " Sef Departament";
                case "functionary":
                    return user.LastName + " " + user.FirstName + " Functionar";
                case "mayor":
                    return user.LastName + " " + user.FirstName + " Primar";
                default:
                    return string.Empty;
            }
        }
    }
}