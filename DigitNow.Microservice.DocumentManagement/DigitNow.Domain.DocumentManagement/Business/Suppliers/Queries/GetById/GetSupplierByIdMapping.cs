using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetById
{
    public class GetSupplierByIdMapping :Profile
    {
        public GetSupplierByIdMapping()
        {
            CreateMap<Supplier, GetSupplierByIdResponse>();
              

        }
    }
}
