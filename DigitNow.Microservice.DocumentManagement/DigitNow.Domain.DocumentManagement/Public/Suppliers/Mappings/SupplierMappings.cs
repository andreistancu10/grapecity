using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Suppliers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Suppliers.Mappings
{
    public class SupplierMappings :Profile
    {
        public SupplierMappings()
        {
            CreateMap<CreateSupplierRequest, CreateSupplierCommand>();
            CreateMap<CreateSupplierLegalRepresentativeRequest, CreateSupplierLegalRepresentative>();
            CreateMap<UpdateSupplierRequest, UpdateSupplierCommand>();
            CreateMap<UpdateSupplierLegalRepresentativeRequest, UpdateSupplierLegalRepresentative>();
        }
        
    }
}
