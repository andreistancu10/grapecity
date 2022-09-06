using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class DynamicFormAggregate
    {
        public DynamicFormFillingLog FormValues { get; set; }
        public IReadOnlyList<UserModel> Users { get; set; }
        public IReadOnlyList<ArchivedDocumentCategoryModel> Categories { get; set; }
        public IReadOnlyList<DepartmentModel> Departments { get; set; }
    }
}
