using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class ActivityAggregate
    {
        public Activity Activity { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
        public IReadOnlyList<DepartmentModel> Departments { get; set; }
        public IReadOnlyList<ObjectiveModel> SpecificObjectives { get; set; }
        public IReadOnlyList<ObjectiveModel> GeneralObjectives { get; set; }
    }
}