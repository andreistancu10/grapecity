﻿using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary
{
    public class FunctionaryAsksForApproval : BaseWorkflowProvider, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await GetDocumentById(command.DocumentId, token);

            if (!Validate(command))
            {
                return command;
            }

            //command.Status = DocumentStatus.InWorkApprovalRequested;
            //command.RecipientHasChanged = true;
            //command.RecipientType = UserRole.HeadOfDepartment;

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
            if (command.Resolution <= 0)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Selected resolution is invalid!",
                    TranslationCode = "dms.resolution.backend.update.validation.resolutionInvalid",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            return true;
        }
    }
}
