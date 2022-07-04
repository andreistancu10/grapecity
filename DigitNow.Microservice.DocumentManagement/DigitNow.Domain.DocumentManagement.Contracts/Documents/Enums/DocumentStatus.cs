
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
        InWorkDelegatedUnallocated = 12
    }
}
