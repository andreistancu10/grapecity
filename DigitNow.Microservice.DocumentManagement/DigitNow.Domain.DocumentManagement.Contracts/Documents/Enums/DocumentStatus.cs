
using System;
using System.Collections.Generic;

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
    
    public static class DocumentStatusMapping
    {
        public static readonly Dictionary<DocumentStatus, string> FileStatusLabels = new()
        {
            { DocumentStatus.InWorkUnallocated, "In lucru nerepartizat" },
            { DocumentStatus.InWorkAllocated, "In lucru alocat" },
            { DocumentStatus.OpinionRequestedUnallocated, "Solicitat opinie nerepartizat" },
            { DocumentStatus.OpinionRequestedAllocated, "Solicitat opinie alocat" },
            { DocumentStatus.InWorkApprovalRequested, "In lucru solicitare aprobare" },
            { DocumentStatus.InWorkMayorReview, "In lucru verificare primar" },
            { DocumentStatus.InWorkCountersignature, "In lucru contrasemnat primar" },
            { DocumentStatus.InWorkMayorDeclined, "In lucru respins primar" },
            { DocumentStatus.Finalized, "Finalizat" },
            { DocumentStatus.NewDeclinedCompetence, "Nou declinat competenta" },
            { DocumentStatus.InWorkDeclined, "In lucru respins" },
            { DocumentStatus.InWorkApproved, "In lucru aprobat" },
            { DocumentStatus.InWorkDelegated, "In lucru delegat" },
            { DocumentStatus.InWorkDelegatedUnallocated, "In lucru delegat nerepartizat" },
            { DocumentStatus.New, "Nou" }
        };
    }
}
