using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class VirtualObjectiveAgregate<T>
        where T : VirtualObjective
    {
        public T VirtualObjective { get; set; }
        internal UserModel CurrentUser { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
        internal IReadOnlyList<DepartmentModel> Departments { get; set; }
    }
}
