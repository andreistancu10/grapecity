using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Factory
{
    public static class WorkflowInitiatorFactory
    {
        private static readonly IDictionary<UserRole, Func<IWorkflowHandler>> factory = new Dictionary<UserRole, Func<IWorkflowHandler>>();
        static WorkflowInitiatorFactory()
        {
            factory.Add(UserRole.HeadOfDepartment, () => new HeadOfDepartment());
            factory.Add(UserRole.Functionary, () => new Functionary());
            factory.Add(UserRole.Mayor, () => new Mayor());
        }

        public static IWorkflowHandler Create(UserRole userRole)
        {
            return factory[userRole]();
        }
    }
}
