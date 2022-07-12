using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager
{
    public abstract class BaseWorkflowManager
    {
        public BaseWorkflowManager(IServiceProvider serviceProvider)
        {
            WorkflowService = serviceProvider.GetService<IWorkflowManagementService>();
            AuthenticationClient = serviceProvider.GetService<IAuthenticationClient>();
            IdentityAdapterClient = serviceProvider.GetService<IIdentityAdapterClient>();
        }

        public readonly IWorkflowManagementService WorkflowService;
        public readonly IAuthenticationClient AuthenticationClient;
        public readonly IIdentityAdapterClient IdentityAdapterClient;

        public async Task<User> FetchHeadOfDepartmentByDepartmentId(long departmentId, CancellationToken token)
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

        protected virtual void TransferResponsibility(WorkflowHistory oldRecord, WorkflowHistory newRecord, ICreateWorkflowHistoryCommand command)
        {
            if (oldRecord == null)
            {
                TransitionNotAllowed(command);
            }

            newRecord.RecipientId = oldRecord.RecipientId;
            newRecord.RecipientType = oldRecord.RecipientType;
            newRecord.RecipientName = oldRecord.RecipientName;
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
                    Parameters = new object[] { }
                });
                return false;
            }
            return true;
        }

        protected async Task PassDocumentToRegistry(Document document, ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var creator = await IdentityAdapterClient.GetUserByIdAsync(document.CreatedBy, token);

            document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(document, UserRole.Functionary, creator, DocumentStatus.NewDeclinedCompetence, command.DeclineReason, command.Remarks));
        }

        protected void PassDocumentToFunctionary(Document document, ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = WorkflowService
                .GetOldWorkflowResponsible(document, x => x.RecipientType == UserRole.Functionary.Id);

            var newWorkflowResponsible = new WorkflowHistory();
            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            newWorkflowResponsible.Status = DocumentStatus.InWorkAllocated;
            newWorkflowResponsible.DeclineReason = command.DeclineReason;
            newWorkflowResponsible.Remarks = command.Remarks;

            document.IncomingDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }
    }
}
