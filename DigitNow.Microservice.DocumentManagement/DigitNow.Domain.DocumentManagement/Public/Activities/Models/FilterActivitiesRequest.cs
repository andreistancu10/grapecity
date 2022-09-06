using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Models
{
    public class FilterActivitiesRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ActivityFilterDto Filter { get; set; }
    }

    public class ActivityFilterDto
    {
    }
}
