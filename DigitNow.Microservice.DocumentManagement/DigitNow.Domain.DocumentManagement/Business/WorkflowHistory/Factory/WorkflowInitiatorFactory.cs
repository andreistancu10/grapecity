using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models;
using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Factory
{
    public static class WorkflowInitiatorFactory
    {
        private static readonly IDictionary<int, Func<IWorkflowHandler>> factory = new Dictionary<int, Func<IWorkflowHandler>>();
        public static IDocumentsQueryService DocumentQueryService { set; get; }

        static WorkflowInitiatorFactory()
        {
            factory.Add(1, () => new HeadOfDepartment(DocumentQueryService));
            factory.Add(2, () => new Functionary(DocumentQueryService));
            factory.Add(3, () => new Mayor(DocumentQueryService));
        }

        public static IWorkflowHandler Create(int typeId)
        {
            return factory[typeId]();
        }
    }
}
