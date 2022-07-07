using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager
{
    public class BaseWorkflowManager
    {
        public static IWorkflowManagementService WorkflowService { get; set; }
        public static IIdentityAdapterClient IdentityService { get; set; }

        public async Task<User> GetUserByIdAsync(long id, CancellationToken token)
        {
            var user = new User() { FirstName = "Ciprian", LastName = "Rosca" };
            return user;
            //return await IdentityService.GetUserByIdAsync(id, token);
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

        public void ResetWorkflowRecord(WorkflowHistory oldRecord, WorkflowHistory newRecord, ICreateWorkflowHistoryCommand command)
        {
            if (oldRecord == null)
            {
                TransitionNotAllowed(command);
            }

            newRecord.RecipientId = oldRecord.RecipientId;
            newRecord.RecipientType = oldRecord.RecipientType;
            newRecord.RecipientName = oldRecord.RecipientName;
        }

        public static void TransitionNotAllowed(ICreateWorkflowHistoryCommand command)
        {
            command.Result = ResultObject.Error(new ErrorMessage
            {
                Message = $"Transition not allwed!",
                TranslationCode = "dms.invalidState.backend.update.validation.invalidState",
                Parameters = new object[] { command.Resolution }
            });
        }

        public static bool UserExists(User user, ICreateWorkflowHistoryCommand command)
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
