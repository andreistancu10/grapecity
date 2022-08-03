using System.Collections.Generic;
using System.Linq;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public enum DocumentStatus
    {
        InWorkUnallocated = 1,
        NewDeclinedCompetence = 2,
        InWorkAllocated = 3,
        OpinionRequestedUnallocated = 4,
        OpinionRequestedAllocated = 5,
        InWorkApprovalRequested = 6,
        InWorkDeclined = 7,
        InWorkApproved = 8,
        InWorkCountersignature = 9,
        Finalized = 10,
        InWorkDelegated = 11,
        InWorkDelegatedUnallocated = 12,
        InWorkMayorReview = 13,
        InWorkMayorDeclined = 14,
        New = 15
    }

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
            { DocumentStatus.InWorkAllocated, "dms.document.status.in-work-Allocated" },
            { DocumentStatus.OpinionRequestedUnallocated, "dms.document.status.opinion-requested-unallocated" },
            { DocumentStatus.OpinionRequestedAllocated, "dms.document.status.opinion-requested-allocated" },
            { DocumentStatus.InWorkApprovalRequested, "dms.document.status.in-work-approval-requested" },
            { DocumentStatus.InWorkMayorReview, "dms.document.status.in-work-mayor-review" },
            { DocumentStatus.InWorkCountersignature, "dms.document.status.in-work-countersignature" },
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

    public class DocumentTypeTranslation
    {
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeLabel { get; set; }

        public DocumentTypeTranslation(DocumentType documentType, string documentTypeLabel)
        {
            DocumentType = documentType;
            DocumentTypeLabel = documentTypeLabel;
        }
    }
    public class DocumentTypeTranslations : List<DocumentTypeTranslation>
    {
        public string GetTranslation(DocumentType documentType)
        {
            var foundDocumentType = this.FirstOrDefault(x => x.DocumentType == documentType);

            if (foundDocumentType != null)
            {
                return foundDocumentType.DocumentTypeLabel;
            }

            return string.Empty;
        }
    }
}
