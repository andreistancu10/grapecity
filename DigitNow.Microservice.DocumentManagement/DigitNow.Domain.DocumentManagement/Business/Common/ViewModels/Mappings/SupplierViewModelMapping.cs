using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class SupplierViewModelMapping :Profile
    {
        public SupplierViewModelMapping()
        {
            CreateMap<Supplier, SupplierViewModel>();
          //CreateMap<List<Supplier>, List<SupplierViewModel> >();

        }
    }
}
