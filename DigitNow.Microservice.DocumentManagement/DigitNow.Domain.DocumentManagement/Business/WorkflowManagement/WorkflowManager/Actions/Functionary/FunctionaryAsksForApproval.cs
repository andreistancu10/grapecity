using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionaryAsksForApproval : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionaryAsksForApproval(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.InWorkAllocated, 
            (int)DocumentStatus.InWorkDelegated, 
            (int)DocumentStatus.OpinionRequestedAllocated, 
            (int)DocumentStatus.New, 
            (int)DocumentStatus.InWorkDeclined 
        };

        #region [ IWorkflowHandler ]

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    return await CreateWorkflowRecordForIncomingAsync(command, document, token);
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    return await CreateWorkflowRecordForInternalOrOutgoingAsync(command, document, token);
                default:
                    return command;
            }
        }

        #endregion

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordForIncomingAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = await GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.HeadOfDepartment.Id, token);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkApprovalRequested,
                Resolution = command.Resolution
            };

            await TransferResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);
            document.WorkflowHistories.Add(newWorkflowResponsible);

            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordForInternalOrOutgoingAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var response = await IdentityAdapterClient.GetUsersAsync(token);
            var departmentUsers = response.Users.Where(x => x.Departments.Contains(document.DestinationDepartmentId));
            var headOfDepartment = departmentUsers.FirstOrDefault(x => x.Roles.Contains(RecipientType.HeadOfDepartment.Code));

            if (headOfDepartment == null)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {document.RecipientId} was found.",
                    TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { document.RecipientId }
                });

                return command;
            }

            document.WorkflowHistories.Add(WorkflowHistoryLogFactory.Create(document, RecipientType.HeadOfDepartment, headOfDepartment, DocumentStatus.InWorkApprovalRequested));

            document.Status = DocumentStatus.InWorkApprovalRequested;
            document.RecipientId = headOfDepartment.Id;

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
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
