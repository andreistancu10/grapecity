using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Models
{
    public class FilterActivitiesRequest : AbstractFilterModel<Activity>
    {
        public long? Id { get; set; }
        public SpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActivitiesFilter ActivitiesFilter { get; set; }
        public DepartmentsFilter DepartmentsFilter { get; set; }
        public FunctionariesFilter FunctionariesFilter { get; set; }
    }

    public class SpecificObjectivesFilter
    {
        public List<long> SpecificObjectivesId { get; set; }
    }

    public class ActivitiesFilter
    {
        public List<long> ActivitiesId { get; set; }
    }

    public class DepartmentsFilter
    {
        public List<long> DepartmentIds { get; set; }
    }

    public class FunctionariesFilter
    {
        public List<long> FunctionariesId { get; set; }
    }
}
