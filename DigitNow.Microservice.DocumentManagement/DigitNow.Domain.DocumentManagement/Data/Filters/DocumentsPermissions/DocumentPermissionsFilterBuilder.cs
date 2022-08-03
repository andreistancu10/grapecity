using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights
{
    internal class DocumentPermissionsFilterBuilder : DataExpressionFilterBuilder<Document, DocumentPermissionsFilter>
    {
        #region [ Properties ]

        private DocumentUserContextFilter UserContextFilter
            => EntityFilter.UserContextFilter;

        private DocumentDepartmentPermissionsFilters DepartmentPermissionsFilters
            => EntityFilter.DepartmentsPermissionsFilter;

        private DocumentRegistryOfficeDepartmentRightsFilter RegistryOfficeDepartmentRightsFilter
            => DepartmentPermissionsFilters?.RegistryOfficeRightsFilter;


        private DocumentUserPermissionsFilters UserPermissionsFilter
            => EntityFilter.UserPermissionsFilter;

        private DocumentMayorPermissionsFilter MayorPermissionsFilter
            => UserPermissionsFilter?.MayorPermissionsFilter;

        private DocumentHeadOfDepartmentPermissionsFilter HeadOfDepartmentPermissionsFilter
            => UserPermissionsFilter?.HeadOfDepartmentPermissionsFilter;

        private DocumentFunctionaryPermissionsFilter FunctionaryPermissionsFilter
            => UserPermissionsFilter?.FunctionaryPermissionsFilter;

        #endregion

        public DocumentPermissionsFilterBuilder(IServiceProvider serviceProvider, DocumentPermissionsFilter filter)
            : base(serviceProvider, filter) { }

        private bool UserIsPartOfDepartments()
        {
            if (DepartmentPermissionsFilters == null)
            {
                return false;
            }

            if (RegistryOfficeDepartmentRightsFilter != null)
            {
                if (RegistryOfficeDepartmentRightsFilter.DepartmentId == UserContextFilter.DepartmentId)
                {
                    return true;
                }
            }

            return false;
        }

        private void BuildRegistryOfficeDepartmentFilter()
        {
            if (RegistryOfficeDepartmentRightsFilter != null)
            {
                if (HeadOfDepartmentPermissionsFilter != null)
                {
                    BuildHeadOfDepartmentFilter();
                }
                else
                {
                    // Incoming => Allow all
                    // Internal => No not allow
                    // Outgoing => Only with Status = Finalized
                    EntityPredicates.Add(x =>
                        (x.DocumentType == DocumentType.Incoming && x.DestinationDepartmentId == RegistryOfficeDepartmentRightsFilter.DepartmentId)
                        ||
                        (x.DocumentType == DocumentType.Outgoing && x.DestinationDepartmentId == RegistryOfficeDepartmentRightsFilter.DepartmentId && x.Status == DocumentStatus.Finalized)
                        // ????? Is this behaviour intended for outgoing ?
                    );
                }
            }
        }

        private void BuildMayorFilter()
        {
            if (MayorPermissionsFilter != null)
            {
                // Allow full access
            }
        }

        private void BuildHeadOfDepartmentFilter()
        {
            if (HeadOfDepartmentPermissionsFilter != null)
            {
                EntityPredicates.Add(x => x.DestinationDepartmentId == HeadOfDepartmentPermissionsFilter.DepartmentId);
            }
        }

        private void BuildFunctionaryFilter()
        {
            if (FunctionaryPermissionsFilter != null)
            {
                // Allow access to it's assigned documents
                EntityPredicates.Add(x =>
                    (x.RecipientId == FunctionaryPermissionsFilter.UserId)
                    &&
                    (x.DestinationDepartmentId == FunctionaryPermissionsFilter.DepartmentId)
                );

                // Allow access for additional documents with the following rules
                EntityPredicates.Add(x =>
                    (x.DestinationDepartmentId == FunctionaryPermissionsFilter.DepartmentId)
                    &&
                    (x.DocumentType == DocumentType.Outgoing || x.DocumentType == DocumentType.Internal)
                    &&
                    (
                        x.Status == DocumentStatus.New
                            ||
                         x.Status == DocumentStatus.InWorkMayorCountersignature
                            ||
                         x.Status == DocumentStatus.InWorkMayorDeclined
                            ||
                         x.Status == DocumentStatus.Finalized
                    )
                );
            }
        }

        protected override void InternalBuild()
        {
            if (UserIsPartOfDepartments())
            {
                BuildRegistryOfficeDepartmentFilter();
            }
            else
            {
                BuildMayorFilter();
                BuildHeadOfDepartmentFilter();
                BuildFunctionaryFilter();
            }
        }
    }
}
