using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights.Preprocess;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    internal static class DataFilterFactory
    {
        public static DocumentDepartmentRightsFilter BuildDocumentDepartmentRightsFilter(UserModel currentUser)
        {
            var filter = new DocumentDepartmentRightsFilter();

            //TODO: Write filter
            filter.RegistryOfficeFilter = new DocumentRegistryOfficeDepartmentFilter
            {
                DepartmentId = default(long),
                DepartmentCode = default(string)
            };

            return filter;
        }

        public static DocumentUserRightsFilter BuildDocumentUserRightsFilter(UserModel currentUser)
        {
            var filter = new DocumentUserRightsFilter();

            if (IsRole(currentUser, RecipientType.Mayor))
            {
                filter.MayorRightFilter = new DocumentMayorRightFilter(); ;
            }
            else if (IsRole(currentUser, RecipientType.HeadOfDepartment))
            {
                filter.HeadOfDepartmentRightsFilter = new DocumentHeadOfDepartmentRightFilter
                {
                    DepartmentId = currentUser.Departments.First() //TODO: Ask this
                };
            }
            else if (IsRole(currentUser, RecipientType.Functionary))
            {
                filter.FunctionaryRightsFilter = new DocumentFunctionaryRightFilter
                {
                    UserId = currentUser.Id,
                    DepartmentId = currentUser.Departments.First() //TODO: Ask this
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
