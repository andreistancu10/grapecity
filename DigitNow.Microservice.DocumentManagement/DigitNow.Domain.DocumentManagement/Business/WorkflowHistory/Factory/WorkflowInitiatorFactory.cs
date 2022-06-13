using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models;
using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Factory
{
    public static class WorkflowInitiatorFactory
    {
        private static readonly IDictionary<int, Func<IWorkflowHandler>> factory = new Dictionary<int, Func<IWorkflowHandler>>();

        static WorkflowInitiatorFactory()
        {
            factory.Add(1, () => new HeadOfDepartment());
            factory.Add(2, () => new Functionary());
            factory.Add(3, () => new Mayor());
        }

        public static IWorkflowHandler Create(int typeId)
        {
            return factory[typeId]();
        }
    }
}
