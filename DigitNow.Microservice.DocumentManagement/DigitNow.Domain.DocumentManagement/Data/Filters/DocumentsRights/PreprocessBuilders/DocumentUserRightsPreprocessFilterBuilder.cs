using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights.Preprocess
{
    internal class DocumentUserRightsPreprocessFilterBuilder : DataExpressionFilterBuilder<Document, DocumentUserRightsFilter>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentUserRightsPreprocessFilterBuilder(IServiceProvider serviceProvider, DocumentUserRightsFilter filter)
            : base(serviceProvider, filter)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        private void BuildMayorFilter()
        {
            if (EntityFilter.MayorRightFilter != null)
            {
                // Allow full access
                EntityPredicates.Add(x => true);
            }
        }

        private void BuildHeadOfDepartmentFilter()
        {
            if (EntityFilter.HeadOfDepartmentRightsFilter != null)
            {
                EntityPredicates.Add(x => x.DestinationDepartmentId == EntityFilter.HeadOfDepartmentRightsFilter.DepartmentId);
            }
        }

        private void BuildFunctionaryFilter()
        {
            if (EntityFilter.FunctionaryRightsFilter != null)
            {
                // Allow access to it's assigned documents
                EntityPredicates.Add(x => x.RecipientId == EntityFilter.FunctionaryRightsFilter.UserId);

                // Allow access for additional documents with the following rules

                //TODO: Add last workflow
                //var lastWorkflowsFound = _dbContext.WorkflowHistoryLogs
                //    .OrderByDescending(x => x.CreatedAt)
                //    .Distinct(x => x.DestinationDepartmentId)
                //    .Select(x => new WorkflowHistoryLog { DocumentId = x.DocumentId, DestinationDepartmentId = x.DestinationDepartmentId })
                //    .FirstOrDefault();

                EntityPredicates.Add(x =>
                    //(x.DestinationDepartmentId == (lastWorkflow).DestinationDepartmentId)
                    //&&
                    (x.DocumentType == DocumentType.Outgoing || x.DocumentType == DocumentType.Internal)
                    &&
                    (
                        (x.Status == DocumentStatus.New)
                            ||
                         (x.Status == DocumentStatus.InWorkCountersignature) //TODO: Ask about this
                            ||
                         (x.Status == DocumentStatus.InWorkMayorDeclined)
                            ||
                         (x.Status == DocumentStatus.Finalized)
                    )
                );
            }
        }

        protected override void InternalBuild()
        {
            BuildMayorFilter();
            BuildHeadOfDepartmentFilter();
            BuildFunctionaryFilter();
        }
    }
}
