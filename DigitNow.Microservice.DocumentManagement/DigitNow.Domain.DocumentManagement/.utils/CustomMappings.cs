using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.utils
{
    public static class CustomMappings
    {
        public static readonly Dictionary<DocumentType, string> DocumentTypeTranslations =
            new()
            {
                { DocumentType.Incoming, "dms.document-type.incoming" },
                { DocumentType.Internal, "dms.document-type.internal" },
                { DocumentType.Outgoing, "dms.document-type.outgoing" },
            };

        public static readonly Dictionary<DocumentStatus, string> DocumentStatusTranslations = new()
        {
            { DocumentStatus.InWorkUnallocated, "dms.document.status.in-work-unallocated" },
            { DocumentStatus.InWorkAllocated, "dms.document.status.in-work-allocated" },
            { DocumentStatus.OpinionRequestedUnallocated, "dms.document.status.opinion-requested-unallocated" },
            { DocumentStatus.OpinionRequestedAllocated, "dms.document.status.opinion-requested-allocated" },
            { DocumentStatus.InWorkApprovalRequested, "dms.document.status.in-work-approval-requested" },
            { DocumentStatus.InWorkMayorReview, "dms.document.status.in-work-mayor-review" },
            { DocumentStatus.InWorkMayorCountersignature, "dms.document.status.in-work-countersignature" },
            { DocumentStatus.InWorkMayorDeclined, "dms.document.status.in-work-mayor-declined" },
            { DocumentStatus.Finalized, "dms.document.status.finalized" },
            { DocumentStatus.NewDeclinedCompetence, "dms.document.status.new-declined-competence" },
            { DocumentStatus.InWorkDeclined, "dms.document.status.in-work-declined" },
            { DocumentStatus.InWorkApproved, "dms.document.status.in-work-approved" },
            { DocumentStatus.InWorkDelegated, "dms.document.status.in-work-delegated" },
            { DocumentStatus.InWorkDelegatedUnallocated, "dms.document.status.in-work-delegated-unallocated" },
            { DocumentStatus.New, "dms.document.status.new" }
        };
    }
}