using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager
{
    public abstract class BaseWorkflowManager
    {
        protected BaseWorkflowManager(IServiceProvider serviceProvider)
        {
            IdentityAdapterClient = serviceProvider.GetService<IIdentityAdapterClient>();
            DbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        public readonly IIdentityAdapterClient IdentityAdapterClient;
        public readonly DocumentManagementDbContext DbContext;
        protected abstract int[] allowedTransitionStatuses { get; }
        protected abstract Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token);

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await DbContext.Documents
                .Include(x => x.WorkflowHistories)
                .FirstAsync(x => x.Id == command.DocumentId);
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

        public async Task SetStatusAndRecipientBasedOnWorkflowDecision(long documentId, long recipientId, DocumentStatus status)
        {
            var document = await DbContext.Documents.FirstAsync(x => x.Id == documentId);

            document.RecipientId = recipientId;
            document.Status = status;
        }

        public async Task<User> FetchHeadOfDepartmentByDepartmentIdAsync(long departmentId, CancellationToken token)
        {
            var response = await IdentityAdapterClient.GetUsersAsync(token);
            var departmentUsers = response.Users.Where(x => x.Departments.Contains(departmentId));
            
            return departmentUsers.FirstOrDefault(x => x.Roles.Contains(RecipientType.HeadOfDepartment.Code));
        }

        public async Task<User> FetchMayorAsync(CancellationToken token)
        {
            var response = await IdentityAdapterClient.GetUsersAsync(token);
            return response.Users.FirstOrDefault(x => x.Roles.Contains(RecipientType.Mayor.Code));
        }

        protected async virtual Task TransferResponsibility(WorkflowHistoryLog oldRecord, WorkflowHistoryLog newRecord, ICreateWorkflowHistoryCommand command)
        {
            if (oldRecord == null)
            {
                TransitionNotAllowed(command);
            }

            newRecord.RecipientId = oldRecord.RecipientId;
            newRecord.RecipientType = oldRecord.RecipientType;
            newRecord.RecipientName = oldRecord.RecipientName;

            await SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, newRecord.RecipientId, newRecord.DocumentStatus);
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
            var creator = await IdentityAdapterClient.GetUserByIdAsync(document.CreatedBy, token);

            var newWorkflowHistoryLog = WorkflowHistoryLogFactory.Create(document.Id, RecipientType.Functionary, creator, DocumentStatus.NewDeclinedCompetence, command.DeclineReason, command.Remarks);
            document.WorkflowHistories.Add(newWorkflowHistoryLog);

            await SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, creator.Id, DocumentStatus.NewDeclinedCompetence);
        }

        protected async Task PassDocumentToFunctionary(Document document, WorkflowHistoryLog newWorkflowResponsible, ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(document, x => x.RecipientType == RecipientType.Functionary.Id);

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        public WorkflowHistoryLog GetOldWorkflowResponsible(Document document, Expression<Func<WorkflowHistoryLog, bool>> predicate)
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
