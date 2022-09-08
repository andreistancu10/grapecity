using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class ActionAggregate
    {
        public Action Action { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
        public IReadOnlyList<DepartmentModel> Departments { get; set; }
        public IReadOnlyList<ObjectiveModel> SpecificObjectives { get; set; }
        public IReadOnlyList<ObjectiveModel> GeneralObjectives { get; set; }
    }
}