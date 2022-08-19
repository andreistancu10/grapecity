using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights;
using DigitNow.Domain.DocumentManagement.extensions.Role;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    internal static class DataFilterFactory
    {
        //TODO: Refactor this method after we will add roles on each department
        public static DocumentUserPermissionsFilters BuildDocumentUserRightsFilter(UserModel currentUser)
        {
            var filter = new DocumentUserPermissionsFilters();

            if (UserExtension.HasRole(currentUser, RecipientType.Mayor))
            {
                filter.MayorPermissionsFilter = new DocumentMayorPermissionsFilter();
            }
            // TODO: If the tests are ok, we can merge this 2 into a common Permission Filter class            
            else if (UserExtension.HasRole(currentUser, RecipientType.HeadOfDepartment))
            {
                filter.HeadOfDepartmentPermissionsFilter = new DocumentHeadOfDepartmentPermissionsFilter
                {
                    UserId = currentUser.Id,
                    DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList()
                };
            }
            else if (UserExtension.HasRole(currentUser, RecipientType.Functionary))
            {
                filter.FunctionaryPermissionsFilter = new DocumentFunctionaryPermissionsFilter
                {
                    UserId = currentUser.Id,
                    DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList()
                };
            }

            return filter;
        }
    }
}
