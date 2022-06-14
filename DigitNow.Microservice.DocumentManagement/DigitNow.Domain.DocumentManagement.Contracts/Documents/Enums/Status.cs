
namespace DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums
{
    public enum Status
    {
        in_work_unallocated = 1,
        new_declined_competence = 2,
        in_work_allocated = 3,
        opinion_requested_unallocated = 4,
        opinition_requested_allocated = 5,
        in_work_approval_requested = 6,
        in_work_declined = 7,
        in_work_approved = 8,
        in_work_countersignature = 9,
        finalized = 10
    }
}
