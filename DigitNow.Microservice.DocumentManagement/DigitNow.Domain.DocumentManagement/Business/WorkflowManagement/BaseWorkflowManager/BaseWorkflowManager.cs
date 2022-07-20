using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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
            IdentityService = serviceProvider.GetService<IdentityService>();
        }

        public readonly IIdentityAdapterClient IdentityAdapterClient;
        public readonly DocumentManagementDbContext DbContext;
        public readonly IdentityService IdentityService;
        protected abstract int[] allowedTransitionStatuses { get; }
        protected abstract Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token);

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await DbContext.Documents.FirstAsync(x => x.Id == command.DocumentId);
            var virtualDocument = await GetVirtualDocumentWorkflowHistoryByIdAsync(document);
            var lastWorkFlowRecord = GetLastWorkflowRecord(virtualDocument);

            await CreateWorkflowRecordInternal(command, document, virtualDocument, lastWorkFlowRecord, token);
            await DbContext.SaveChangesAsync(token);

            return command;
        }

        protected async Task<User> GetCurrentUser(CancellationToken token)
        {
            var currentUserId = IdentityService.GetCurrentUserId();
            return await IdentityAdapterClient.GetUserByIdAsync(currentUserId, token);
        }

        public WorkflowHistory GetLastWorkflowRecord(VirtualDocument document)
        {
            return document.WorkflowHistory.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }

        public static bool IsTransitionAllowed(WorkflowHistory lastWorkFlowRecord, int[] allowedTransitionStatuses)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.Status))
            {
                return false;
            }
            return true;
        }

        protected async Task<VirtualDocument> GetVirtualDocumentWorkflowHistoryByIdAsync(Document document)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    return await DbContext.IncomingDocuments.Include(x => x.WorkflowHistory).FirstOrDefaultAsync(x => x.DocumentId == document.Id);
                case DocumentType.Internal:
                    return await DbContext.InternalDocuments.Include(x => x.WorkflowHistory).FirstOrDefaultAsync(x => x.DocumentId == document.Id);
                case DocumentType.Outgoing:
                    return await DbContext.OutgoingDocuments.Include(x => x.WorkflowHistory).FirstOrDefaultAsync(x => x.DocumentId == document.Id);
                default:
                    return null;
            }
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
            
            return departmentUsers.FirstOrDefault(x => x.Roles.Contains(UserRole.HeadOfDepartment.Code));
        }

        public async Task<User> FetchMayorAsync(CancellationToken token)
        {
            var response = await IdentityAdapterClient.GetUsersAsync(token);
            return response.Users.FirstOrDefault(x => x.Roles.Contains(UserRole.Mayor.Code));
        }

        protected async virtual Task TransferResponsibility(WorkflowHistory oldRecord, WorkflowHistory newRecord, ICreateWorkflowHistoryCommand command)
        {
            if (oldRecord == null)
            {
                TransitionNotAllowed(command);
            }

            newRecord.RecipientId = oldRecord.RecipientId;
            newRecord.RecipientType = oldRecord.RecipientType;
            newRecord.RecipientName = oldRecord.RecipientName;

            await SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, newRecord.RecipientId, newRecord.Status);
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

        protected async Task PassDocumentToRegistry(VirtualDocument document, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var creator = await IdentityAdapterClient.GetUserByIdAsync(document.CreatedBy, token);

            document.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(UserRole.Functionary, creator, DocumentStatus.NewDeclinedCompetence, command.DeclineReason, command.Remarks));

            await SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, creator.Id, DocumentStatus.NewDeclinedCompetence);
        }

        protected async Task PassDocumentToFunctionary(VirtualDocument document, WorkflowHistory newWorkflowResponsible, ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(document, x => x.RecipientType == UserRole.Functionary.Id);

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            document.WorkflowHistory.Add(newWorkflowResponsible);
        }

        public WorkflowHistory GetOldWorkflowResponsible(VirtualDocument document, Expression<Func<WorkflowHistory, bool>> predicate)
        {
            return ExtractResponsible(document.WorkflowHistory, predicate);
        }

        private static WorkflowHistory ExtractResponsible(List<WorkflowHistory> history, Expression<Func<WorkflowHistory, bool>> predicate)
        {
            return history.AsQueryable()
                          .Where(predicate)
                          .OrderByDescending(x => x.CreatedAt)
                          .FirstOrDefault();
        }
    }
}
