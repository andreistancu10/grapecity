using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models;

public class VirtualDocumentAggregate<T>
    where T : VirtualDocument
{
    public T VirtualDocument { get; set; }
    internal IReadOnlyList<UserModel> Users { get; set; }
    internal IReadOnlyList<DocumentCategoryModel> Categories { get; set; }
    internal IReadOnlyList<DocumentCategoryModel> InternalCategories { get; set; }
    public IReadOnlyList<DocumentDepartmentModel> Departments { get; set; }
    public DocumentsSpecialRegisterMappingModel? SpecialRegisterMapping { get; set; }
}