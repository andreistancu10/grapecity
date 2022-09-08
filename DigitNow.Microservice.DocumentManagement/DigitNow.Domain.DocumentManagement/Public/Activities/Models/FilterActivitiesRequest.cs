using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Models
{
    public class FilterActivitiesRequest : AbstractFilterModel<Activity>
    {
        public long? Id { get; set; }
    }
}
