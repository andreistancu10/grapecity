using DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers;
using HTSS.Platform.Core.CQRS;


namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetFilteredSuppliers
{
    public class GetFilteredSuppliersQuery : IQuery<ResultObject>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public SupplierFilter Filter { get; set; }
    }
}
