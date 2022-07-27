using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.extensions.Role
{
    public static class RoleExtension
    {
        public static bool HasRole(this UserModel userModel, RecipientType role)
        {
            return userModel.Roles.Contains(role.Code);
        }
    }
}
