using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.WorkflowEditors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    public class WorkflowInitiatorFactory
    {
        private static readonly IDictionary<UserRole, Func<IWorkflowHandler>> factory = new Dictionary<UserRole, Func<IWorkflowHandler>>();
        public WorkflowInitiatorFactory(IServiceProvider serviceProvider)
        {
            factory.Clear();

            factory.Add(UserRole.HeadOfDepartment, () => new HeadOfDepartment(serviceProvider));
            factory.Add(UserRole.Functionary, () => new Functionary(serviceProvider));
            factory.Add(UserRole.Mayor, () => new Mayor(serviceProvider));
        }

        public IWorkflowHandler Create(UserRole userRole)
        {
            return factory[userRole]();
        }
    }
}
