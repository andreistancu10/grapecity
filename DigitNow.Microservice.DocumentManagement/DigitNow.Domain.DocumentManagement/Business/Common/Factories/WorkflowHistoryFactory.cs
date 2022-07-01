
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.extensions.Autocorrect;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    public static class WorkflowHistoryFactory
    {
        public static WorkflowHistory Create(Document document, UserRole role, User user, DocumentStatus documentStatus, string declineReason = "", string remarks = "")
            => new WorkflowHistory {
                RecipientType = (int)role,
                RecipientId = user.Id,
                RecipientName = user.FormatUserNameByRole(role),
                Status = (int)documentStatus,
                RegistrationNumber = document.RegistrationNumber,
                DeclineReason = declineReason,
                Remarks = remarks
            };
    }
}
