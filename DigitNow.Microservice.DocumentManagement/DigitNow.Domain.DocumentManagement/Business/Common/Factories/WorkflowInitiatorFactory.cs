using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.WorkflowEditors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
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
