using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Models
{
    public class FilterActionsRequest : AbstractFilterModel<Data.Entities.Action>
    {
        public long ActivityId { get; set; }
    }
}
