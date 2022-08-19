using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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
        protected readonly IAuthenticationClient AuthenticationClient;
        protected readonly IIdentityService IdentityService;
        protected readonly ICatalogAdapterClient CatalogAdapterClient;
        protected readonly IMailSenderService MailSenderService;

        protected BaseWorkflowManager(IServiceProvider serviceProvider)
        {
            DbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            AuthenticationClient = serviceProvider.GetService<IAuthenticationClient>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
            CatalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
            MailSenderService = serviceProvider.GetService<IMailSenderService>();
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
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.Document.Status))
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
                document.RecipientId = headOfDepartment?.Id;
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

        protected virtual bool UserExists(UserModel user, ICreateWorkflowHistoryCommand command)
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
            var registry = await CatalogAdapterClient.GetDepartmentByCodeAsync(UserDepartment.Registry.Code, token);

            await CreateMetadata(document, command, registry, token);
        }

        protected async Task PassDocumentToDepartment(Document document, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var department = await CatalogAdapterClient.GetDepartmentByIdAsync(document.DestinationDepartmentId, token);

            await CreateMetadata(document, command, department, token);
        }

        private async Task CreateMetadata(Document document, ICreateWorkflowHistoryCommand command, Adapters.MS.Catalog.Poco.Department department, CancellationToken token)
        {
            var newWorkflowHistoryLog = new WorkflowHistoryLog
            {
                DeclineReason = command.DeclineReason,
                DocumentStatus = document.Status,
                RecipientType = RecipientType.Department.Id,
                RecipientId = department.Id,
                RecipientName = $"Departamentul {department.Name}",
                DestinationDepartmentId = (int)department.Id,
                Remarks = command.Remarks,
                OpinionRequestedUntil = command.OpinionRequestedUntil,
                Resolution = command.Resolution
            };

            document.WorkflowHistories.Add(newWorkflowHistoryLog);

            await UpdateDocumentBasedOnWorkflowDecisionAsync(makeDocumentVisibleForDepartment: true, command.DocumentId, department.Id, document.Status, token);
        }

        protected async Task PassDocumentToResponsibleUserAsync(Document document, WorkflowHistoryLog newWorkflowResponsible, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.Functionary.Id 
                    || (x.RecipientType == RecipientType.HeadOfDepartment.Id && x.DocumentStatus != DocumentStatus.OpinionRequestedUnallocated));

            newWorkflowResponsible.DocumentStatus = oldWorkflowResponsible.RecipientType == RecipientType.HeadOfDepartment.Id 
                ? DocumentStatus.InWorkUnallocated 
                : DocumentStatus.InWorkAllocated;

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        protected static WorkflowHistoryLog GetOldWorkflowResponsibleAsync(Document document, Expression<Func<WorkflowHistoryLog, bool>> predicate)
        {
            return ExtractResponsible(document.WorkflowHistories, predicate);
        }

        private static WorkflowHistoryLog ExtractResponsible(List<WorkflowHistoryLog> history, Expression<Func<WorkflowHistoryLog, bool>> predicate)
        {
            return history.AsQueryable()
                          .Where(predicate)
                          .OrderByDescending(x => x.CreatedAt)
                          .FirstOrDefault();
        }
    }
}
