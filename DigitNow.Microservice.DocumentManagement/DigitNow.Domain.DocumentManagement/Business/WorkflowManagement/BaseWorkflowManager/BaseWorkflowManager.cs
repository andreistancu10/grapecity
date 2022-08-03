using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager
{
    public abstract class BaseWorkflowManager
    {
        protected readonly DocumentManagementDbContext DbContext;
        protected readonly IIdentityAdapterClient IdentityAdapterClient;
        protected readonly IIdentityService IdentityService;
        protected readonly ICatalogAdapterClient CatalogAdapterClient;

        protected BaseWorkflowManager(IServiceProvider serviceProvider)
        {
            DbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            IdentityAdapterClient = serviceProvider.GetService<IIdentityAdapterClient>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
            CatalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
        }

        protected abstract int[] allowedTransitionStatuses { get; }
        protected abstract Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token);

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await DbContext.Documents
                .Include(x => x.WorkflowHistories)
                .FirstAsync(x => x.Id == command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(document);
            
            await CreateWorkflowRecordInternal(command, document, lastWorkFlowRecord, token);
            await DbContext.SaveChangesAsync(token);

            return command;
        }

        public static WorkflowHistoryLog GetLastWorkflowRecord(Document document)
        {
            return document.WorkflowHistories.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }

        public static bool IsTransitionAllowed(WorkflowHistoryLog lastWorkFlowRecord, int[] allowedTransitionStatuses)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.DocumentStatus))
            {
                return false;
            }
            return true;
        }

        public async Task UpdateDocumentBasedOnWorkflowDecisionAsync(bool makeDocumentVisibleForDepartment, long documentId, long recipientId, DocumentStatus status, CancellationToken token)
        {
            var document = await DbContext.Documents.FirstAsync(x => x.Id == documentId, token);

            if (makeDocumentVisibleForDepartment)
            {
                document.DestinationDepartmentId = recipientId;
                var headOfDepartment = await IdentityService.GetHeadOfDepartmentUserAsync(recipientId, token);
                document.RecipientId = headOfDepartment.Id;
            }
            else
            {
                document.RecipientId = recipientId;
            }

            document.Status = status;
        }

        protected async virtual Task TransferUserResponsibilityAsync(WorkflowHistoryLog oldRecord, WorkflowHistoryLog newRecord, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            if (oldRecord == null)
            {
                TransitionNotAllowed(command);
            }

            newRecord.RecipientId = oldRecord.RecipientId;
            newRecord.RecipientType = oldRecord.RecipientType;
            newRecord.RecipientName = oldRecord.RecipientName;

            await UpdateDocumentBasedOnWorkflowDecisionAsync(makeDocumentVisibleForDepartment: false, command.DocumentId, newRecord.RecipientId, newRecord.DocumentStatus, token);
        }

        protected virtual void TransitionNotAllowed(ICreateWorkflowHistoryCommand command)
        {
            command.Result = ResultObject.Error(new ErrorMessage
            {
                Message = $"Transition not allwed!",
                TranslationCode = "dms.invalidState.backend.update.validation.invalidState",
                Parameters = new object[] { command.Resolution }
            });
        }

        protected virtual bool UserExists(User user, ICreateWorkflowHistoryCommand command)
        {
            if (user == null)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible User was found.",
                    TranslationCode = "catalog.user.backend.update.validation.entityNotFound",
                    Parameters = Array.Empty<object>()
                });
                return false;
            }
            return true;
        }

        protected async Task PassDocumentToRegistry(Document document, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var registryDepartment = await CatalogAdapterClient.GetDepartmentByCodeAsync(UserDepartment.Registry.Code, token);

            var newWorkflowHistoryLog = new WorkflowHistoryLog
            {
                DeclineReason = command.DeclineReason,
                DocumentStatus = document.Status,
                RecipientType = RecipientType.Department.Id,
                RecipientId = registryDepartment.Id,
                RecipientName = $"Departamentul {registryDepartment.Name}",
                DestinationDepartmentId = (int)registryDepartment.Id,
                Remarks = command.Remarks
            };

            document.WorkflowHistories.Add(newWorkflowHistoryLog);

            await UpdateDocumentBasedOnWorkflowDecisionAsync(makeDocumentVisibleForDepartment: true, command.DocumentId, registryDepartment.Id, document.Status, token);
        }

        protected async Task PassDocumentToFunctionaryAsync(Document document, WorkflowHistoryLog newWorkflowResponsible, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.Functionary.Id, token);
            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        protected static WorkflowHistoryLog GetOldWorkflowResponsibleAsync(Document document, Expression<Func<WorkflowHistoryLog, bool>> predicate, CancellationToken token)
        {
            return ExtractResponsibleAsync(document.WorkflowHistories, predicate, token);
        }

        private static WorkflowHistoryLog ExtractResponsibleAsync(List<WorkflowHistoryLog> history, Expression<Func<WorkflowHistoryLog, bool>> predicate, CancellationToken token)
        {
            return history.AsQueryable()
                          .Where(predicate)
                          .OrderByDescending(x => x.CreatedAt)
                          .FirstOrDefault();
        }

        public async Task<string> GetDocumentNameByIdAsync(long id, CancellationToken token)
        {
            var department = await CatalogAdapterClient.GetDepartmentByIdAsync(id, token);
            return department.Name;
        }
    }
}
