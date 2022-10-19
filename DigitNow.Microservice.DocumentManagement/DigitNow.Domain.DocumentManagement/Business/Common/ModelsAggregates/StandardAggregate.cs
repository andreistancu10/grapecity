using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class StandardAggregate
    {
        public Standard Standard { get; set; }
        public IReadOnlyList<DepartmentModel> Departments { get; set; }
    }
}
