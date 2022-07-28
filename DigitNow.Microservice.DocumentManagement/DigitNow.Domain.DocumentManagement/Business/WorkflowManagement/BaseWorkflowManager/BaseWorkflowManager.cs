using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
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

        protected BaseWorkflowManager(IServiceProvider serviceProvider)
        {
            DbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            IdentityAdapterClient = serviceProvider.GetService<IIdentityAdapterClient>();
            IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        protected abstract int[] allowedTransitionStatuses { get; }
        protected abstract Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token);

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

        public WorkflowHistoryLog GetLastWorkflowRecord(Document document)
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

        public async Task SetStatusAndRecipientBasedOnWorkflowDecisionAsync(long documentId, long recipientId, DocumentStatus status, CancellationToken token)
        {
            var document = await DbContext.Documents.FirstAsync(x => x.Id == documentId, token);

            document.RecipientId = recipientId;
            document.Status = status;
        }

        protected async virtual Task TransferResponsibilityAsync(WorkflowHistoryLog oldRecord, WorkflowHistoryLog newRecord, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            if (oldRecord == null)
            {
                TransitionNotAllowed(command);
            }

            newRecord.RecipientId = oldRecord.RecipientId;
            newRecord.RecipientType = oldRecord.RecipientType;
            newRecord.RecipientName = oldRecord.RecipientName;

            await SetStatusAndRecipientBasedOnWorkflowDecisionAsync(command.DocumentId, newRecord.RecipientId, newRecord.DocumentStatus, token);
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

        protected async Task PassDocumentToRegistryAsync(Document document, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var creator = await IdentityAdapterClient.GetUserByIdAsync(document.CreatedBy, token);

            var newWorkflowHistoryLog = WorkflowHistoryLogFactory.Create(document.Id, RecipientType.Functionary, creator, DocumentStatus.NewDeclinedCompetence, command.DeclineReason, command.Remarks);
            document.WorkflowHistories.Add(newWorkflowHistoryLog);

            await SetStatusAndRecipientBasedOnWorkflowDecisionAsync(command.DocumentId, creator.Id, DocumentStatus.NewDeclinedCompetence, token);
        }

        protected async Task PassDocumentToFunctionaryAsync(Document document, WorkflowHistoryLog newWorkflowResponsible, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var oldWorkflowResponsible = await GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.Functionary.Id, token);

            await TransferResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        public static Task<WorkflowHistoryLog> GetOldWorkflowResponsibleAsync(Document document, Expression<Func<WorkflowHistoryLog, bool>> predicate, CancellationToken token)
        {
            return ExtractResponsibleAsync(document.WorkflowHistories, predicate, token);
        }

        private static Task<WorkflowHistoryLog> ExtractResponsibleAsync(List<WorkflowHistoryLog> history, Expression<Func<WorkflowHistoryLog, bool>> predicate, CancellationToken token)
        {
            return history.AsQueryable()
                          .Where(predicate)
                          .OrderByDescending(x => x.CreatedAt)
                          .FirstOrDefaultAsync(token);
        }
    }
}
