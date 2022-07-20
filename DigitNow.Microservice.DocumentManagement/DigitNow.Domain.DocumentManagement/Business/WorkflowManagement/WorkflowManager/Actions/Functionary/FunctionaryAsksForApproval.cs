using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionaryAsksForApproval : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionaryAsksForApproval(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated, (int)DocumentStatus.New, (int)DocumentStatus.InWorkDeclined };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            if (document.DocumentType == DocumentType.Internal || document.DocumentType == DocumentType.Outgoing)
            {
                await CreateWorkflowRecordForInternalOrOutgoing(command, document, virtualDocument, token);
                return command;
            }

            var oldWorkflowResponsible = GetOldWorkflowResponsible(virtualDocument, x => x.RecipientType == UserRole.HeadOfDepartment.Id);

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkApprovalRequested,
                Resolution = command.Resolution
            };

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);
            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);

            return command;
        }

        private async Task CreateWorkflowRecordForInternalOrOutgoing(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, CancellationToken token)
        {
            var response = await IdentityAdapterClient.GetUsersAsync(token);
            var departmentUsers = response.Users.Where(x => x.Departments.Contains(document.RecipientId));
            var headOfDepartment = departmentUsers.FirstOrDefault(x => x.Roles.Contains(UserRole.HeadOfDepartment.Code));

            if (headOfDepartment == null)
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {document.RecipientId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { document.RecipientId }
                });

            virtualDocument.WorkflowHistory.Add(WorkflowHistoryFactory
            .Create(UserRole.HeadOfDepartment, headOfDepartment, DocumentStatus.InWorkApprovalRequested));
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (command.Resolution <= 0)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Resolution was not set!",
                    TranslationCode = "dms.resolution.backend.update.validation.notSet",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            return true;
        }
    }
}
