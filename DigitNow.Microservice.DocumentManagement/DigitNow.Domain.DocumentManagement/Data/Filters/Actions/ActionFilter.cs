using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Filters.Common;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Actions
{
    public class ActionFilter : AbstractFilterModel<Action>, IQuery<PagedList<ActionViewModel>>
    {
        public SpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActionsFilter ActionsFilter { get; set; }
        public DepartmentsFilter DepartmentsFilter { get; set; }
        public FunctionariesFilter FunctionariesFilter { get; set; }
        public ActivitiesFilter ActivitiesFilter { get; set; }
    }
}