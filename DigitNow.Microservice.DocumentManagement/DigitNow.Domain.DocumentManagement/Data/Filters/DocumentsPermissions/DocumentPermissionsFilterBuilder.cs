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
                    EntityPredicates.Add(x =>
                        x.DestinationDepartmentId == RegistryOfficeDepartmentRightsFilter.DepartmentId
                        || x.DocumentActions.Select(x => x.DepartmentId).Contains(RegistryOfficeDepartmentRightsFilter.DepartmentId)
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
                EntityPredicates.Add(x => HeadOfDepartmentPermissionsFilter.DepartmentIds.Contains(x.DestinationDepartmentId)
                || x.CreatedBy == HeadOfDepartmentPermissionsFilter.UserId
                || x.RecipientId == HeadOfDepartmentPermissionsFilter.UserId
                || x.DocumentActions.Select(x => x.ResposibleId).Contains(HeadOfDepartmentPermissionsFilter.UserId)
                || x.DocumentActions.Where(x => x.DepartmentId.HasValue).Select(x => x.DepartmentId.Value).Contains(HeadOfDepartmentPermissionsFilter.DepartmentIds.First()));
            }
        }

        private void BuildFunctionaryFilter()
        {
            if (FunctionaryPermissionsFilter != null)
            {
                // Allow access to it's assigned documents
                EntityPredicates.Add(x =>
                    x.RecipientId == FunctionaryPermissionsFilter.UserId || x.CreatedBy == FunctionaryPermissionsFilter.UserId
                    ||
                    x.DocumentActions.Select(x => x.ResposibleId).Contains(FunctionaryPermissionsFilter.UserId)
                    || 
                    x.DocumentActions.Where(x => x.DepartmentId.HasValue).Select(x => x.DepartmentId.Value).Contains(FunctionaryPermissionsFilter.DepartmentIds.First())
                    ||
                    (FunctionaryPermissionsFilter.DepartmentIds.Contains(x.DestinationDepartmentId))
                    &&
                    (
                        (
                            (x.DocumentType == DocumentType.Incoming)
                            &&
                            (
                                x.Status == DocumentStatus.InWorkAllocated
                                    ||
                                x.Status == DocumentStatus.OpinionRequestedAllocated
                                    ||
                                x.Status == DocumentStatus.InWorkDeclined
                                    ||
                                x.Status == DocumentStatus.InWorkMayorCountersignature
                                    ||
                                x.Status == DocumentStatus.Finalized
                            )
                        )
                        ||
                        (
                            (x.DocumentType == DocumentType.Internal || x.DocumentType == DocumentType.Outgoing)
                            &&
                            (
                                x.Status == DocumentStatus.New
                                    ||
                                x.Status == DocumentStatus.OpinionRequestedAllocated
                                    ||
                                x.Status == DocumentStatus.InWorkDeclined
                                    ||
                                 x.Status == DocumentStatus.InWorkMayorCountersignature
                                    ||
                                 x.Status == DocumentStatus.Finalized
                            )
                        )
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
