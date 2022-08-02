using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.extensions.Autocorrect;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories
{
    public static class WorkflowHistoryLogFactory
    {
        public static WorkflowHistoryLog Create(Document document, RecipientType role, User user, DocumentStatus documentStatus, string declineReason = default, string remarks = default, DateTime? opinionRequestedUntil = null, int? resolution = null)
            => new WorkflowHistoryLog {
                DocumentId = document.Id,
                DocumentStatus = documentStatus,
                RecipientType = role.Id,
                RecipientId = user.Id,
                RecipientName = user.FormatUserNameByRole(role.Code),                
                DeclineReason = declineReason,
                Remarks = remarks,
                OpinionRequestedUntil = opinionRequestedUntil,
                Resolution = resolution,
                DestinationDepartmentId = document.DestinationDepartmentId,
            };
    }
}
