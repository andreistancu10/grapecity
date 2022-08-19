using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.extensions.Role
{
    public static class UserExtension
    {
        public static bool HasRole(this UserModel userModel, RecipientType role)
        {
            return userModel.Roles.Select(x => x.Code).Contains(role.Code);
        }
    }
}
