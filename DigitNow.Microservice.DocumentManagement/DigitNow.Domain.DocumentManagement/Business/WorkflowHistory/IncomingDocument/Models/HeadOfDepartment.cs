﻿using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.HeadOfDepartment;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Models
{
    public class HeadOfDepartment : IWorkflowHandler
    {
        private enum ActionType { Allocate, Decline, MakeDecision };

        private Dictionary<ActionType, IWorkflowHandler> actionStrategy
            = new Dictionary<ActionType, IWorkflowHandler>();

        public HeadOfDepartment()
        {
            actionStrategy.Add(ActionType.Allocate, new HeadOfDepartmentAllocatesRequest());
            actionStrategy.Add(ActionType.Decline, new HeadOfDepartmentDeclines());
            actionStrategy.Add(ActionType.MakeDecision, new HeadOfDepartmentMakesDecision());
        }
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var actionKey = (ActionType)command.ActionType;
            if (!actionStrategy.ContainsKey(actionKey))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No Action with id {command.ActionType} is possible for a head of department.",
                    TranslationCode = "dms.actionNotAllowed.backend.update.validation.actionNotAllowed",
                    Parameters = new object[] { command.ActionType }
                });
                return command;
            }

            return await actionStrategy[actionKey].CreateWorkflowRecord(command, token);
        }
    }
}
