using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetById
{
    public class GetSupplierByIdQuery : IQuery<GetSupplierByIdResponse>
    {
        public long Id { get; set; }
        public GetSupplierByIdQuery(long id)
        {
            Id = id;    
        }
    }
}
