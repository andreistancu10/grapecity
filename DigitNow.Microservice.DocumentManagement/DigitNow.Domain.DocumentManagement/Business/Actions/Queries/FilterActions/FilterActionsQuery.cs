using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions
{
    public class FilterActionsQuery : AbstractFilterModel<Action>, IQuery<ResultPagedList<ActionViewModel>>
    {
        public long ActivityId { get; set; }
    }
}
