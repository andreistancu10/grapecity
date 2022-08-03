﻿using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    internal static class DataFilterFactory
    {
        public static DocumentUserPermissionsFilters BuildDocumentUserRightsFilter(UserModel currentUser)
        {
            var filter = new DocumentUserPermissionsFilters();

            if (IsRole(currentUser, RecipientType.Mayor))
            {
                filter.MayorPermissionsFilter = new DocumentMayorPermissionsFilter(); ;
            }
            else if (IsRole(currentUser, RecipientType.HeadOfDepartment))
            {
                filter.HeadOfDepartmentPermissionsFilter = new DocumentHeadOfDepartmentPermissionsFilter
                {
                    // TODO:(!) Each user will be assigned to only one department
                    DepartmentId = currentUser.Departments.First() 
                };
            }
            else if (IsRole(currentUser, RecipientType.Functionary))
            {
                filter.FunctionaryPermissionsFilter = new DocumentFunctionaryPermissionsFilter
                {
                    UserId = currentUser.Id,
                    // TODO:(!) Each user will be assigned to only one department
                    DepartmentId = currentUser.Departments.First()
                };
            }

            return filter;
        }

        #region [ Utils ]

        private static bool IsRole(UserModel userModel, RecipientType role) =>
            userModel.Roles.Contains(role.Code);

        #endregion
    }
}
