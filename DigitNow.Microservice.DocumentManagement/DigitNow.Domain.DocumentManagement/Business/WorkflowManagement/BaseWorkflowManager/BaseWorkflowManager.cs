using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public async Task<User> GetUserByIdAsync(long id, CancellationToken token)
        {
            var user = new User() { FirstName = "Ciprian", LastName = "Rosca" };
            return user;
             //return await IdentityService.GetUserByIdAsync(id, token);

            var users = await AuthenticationClient.GetUserById(id, token);
        }

        public async Task<User> GetUserByDepartmentIdAsync(long id, CancellationToken token)
        {
            var user = new User() { FirstName = "Ciprian", LastName = "Rosca" };
            return user;
            //return await IdentityService.GetUserByDepartmentIdAsync(id, token);
        }

        public async Task<User> GetMayorAsync(int id, CancellationToken token)
        {
            var user = new User() { FirstName = "Ciprian", LastName = "Rosca" };
            return user;

            //var users = await IdentityService.GetUsersByDepartmentIdAsync(id, token);
            //return users.Users.FirstOrDefault(x => x.Roles.Contains((long)UserRole.Mayor));
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
    }
}
