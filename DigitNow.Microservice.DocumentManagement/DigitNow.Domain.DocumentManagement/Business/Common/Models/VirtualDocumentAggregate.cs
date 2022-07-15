using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models;

public class VirtualDocumentAggregate<T>
    where T : VirtualDocument
{
    public T VirtualDocument { get; set; }
    public IReadOnlyList<User> Users { get; set; }
    public IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
    public IReadOnlyList<InternalDocumentCategoryModel> InternalCategories { get; set; }
    public IReadOnlyList<DepartmentModel> Departments { get; set; }
    public SpecialRegisterMappingModel? SpecialRegisterMapping { get; set; }
}
namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class VirtualDocumentAggregate<T>
        where T : VirtualDocument
    {
        public T VirtualDocument { get; set; }
        internal IReadOnlyList<UserModel> Users { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
        internal IReadOnlyList<DocumentCategoryModel> InternalCategories { get; set; }
    }
}
