using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class DynamicFormAggregate
    {
        public DynamicFormFillingLog DynamicFormFillingLog { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
        public IReadOnlyList<DocumentDepartmentModel> Departments { get; set; }
    }
}
