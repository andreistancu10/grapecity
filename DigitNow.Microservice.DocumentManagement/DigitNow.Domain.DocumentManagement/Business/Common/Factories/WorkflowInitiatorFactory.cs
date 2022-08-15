using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.WorkflowEditors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    public class WorkflowInitiatorFactory
    {
        private static readonly IDictionary<int, Func<IWorkflowHandler>> factory = new Dictionary<int, Func<IWorkflowHandler>>();
        public WorkflowInitiatorFactory(IServiceProvider serviceProvider, IMailSenderService mailSenderService)
        {
            factory.Clear();

            factory.Add(RecipientType.HeadOfDepartment.Id, () => new HeadOfDepartment(serviceProvider, mailSenderService));
            factory.Add(RecipientType.Functionary.Id, () => new Functionary(serviceProvider, mailSenderService));
            factory.Add(RecipientType.Mayor.Id, () => new Mayor(serviceProvider, mailSenderService));
        }

        public IWorkflowHandler Create(int userRole)
        {
            return factory[userRole]();
        }
    }
}
