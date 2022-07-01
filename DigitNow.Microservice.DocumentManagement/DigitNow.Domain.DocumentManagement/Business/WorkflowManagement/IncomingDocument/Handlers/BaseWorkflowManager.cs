using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Handlers
{
    public class BaseWorkflowManager
    {
        public static IDocumentService DocumentService { get; set; }
        public static IIdentityAdapterClient IdentityService { get; set; }

        public async Task<Document> GetDocumentById(long id, CancellationToken token)
        {
            var document = await DocumentService.FindAsync(doc => doc.Id == id, token, x => x.IncomingDocument.WorkflowHistory);
            return document;
        }

        public bool ValidateTransitionStatus(WorkflowHistory record, int[] allowedStatusesForTransition)
        {
            if (record == null || !allowedStatusesForTransition.Contains(record.Status))
            {
                return false;
            }
            return true;
        }

        public WorkflowHistory GetLastWorkflowRecord(Document document)
        {
            if (document.IncomingDocument != null)
                return document.IncomingDocument.WorkflowHistory.OrderByDescending(x => x.CreationDate).FirstOrDefault();

            if (document.OutgoingDocument != null)
                return document.OutgoingDocument.WorkflowHistory.OrderByDescending(x => x.CreationDate).FirstOrDefault();

            return null;
        }

        public async Task<User> GetFunctionaryByIdAsync(long id, CancellationToken token)
        {
            var user = new User() { FirstName = "Ciprian", LastName = "Rosca" };
            return user;
            //return await IdentityService.GetUserByIdAsync(id, token);
        }

        public async Task<User> GetMayorAsync(int id, CancellationToken token)
        {
            var user = new User() { FirstName = "Ciprian", LastName = "Rosca" };
            return user;

            //var users = await IdentityService.GetUsersByDepartmentIdAsync(id, token);
            //return users.Users.FirstOrDefault(x => x.Roles.Contains((long)UserRole.Mayor));
        }

        public async Task SaveDocument(CancellationToken token)
        {
            await DocumentService.CommitChangesAsync(token);
        }

        public bool IsTransitionAllowed(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord, int[] allowedTransitionStatuses)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains(lastWorkFlowRecord.Status))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Transition not allwed!",
                    TranslationCode = "dms.invalidState.backend.update.validation.invalidState",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }
            return true;
        }

        public bool UserExists(User user, ICreateWorkflowHistoryCommand command)
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
