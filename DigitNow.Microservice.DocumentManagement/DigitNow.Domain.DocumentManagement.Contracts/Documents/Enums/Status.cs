
namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public enum Status
    {
        inWorkUnallocated = 1,
        newDeclinedCompetence = 2,
        inWorkAllocated = 3,
        opinionRequestedUnallocated = 4,
        opinitionRequestedAllocated = 5,
        inWorkApprovalRequested = 6,
        inWorkDeclined = 7,
        inWorkApproved = 8,
        inWorkCountersignature = 9,
        finalized = 10,
        inWorkDelegated = 11,
        inWorkDelegatedUnallocated = 12
    }
}
