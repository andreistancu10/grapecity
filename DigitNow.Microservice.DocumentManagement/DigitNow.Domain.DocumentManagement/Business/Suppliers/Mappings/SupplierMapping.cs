using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;


namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Mappings
{
    public class SupplierMapping : Profile
    {
        public SupplierMapping()
        {
            CreateMap<CreateSupplierCommand, Supplier>();
            CreateMap<CreateSupplierLegalRepresentative, SupplierLegalRepresentative>();
            CreateMap<UpdateSupplierCommand, Supplier>();
            CreateMap<UpdateSupplierLegalRepresentative, SupplierLegalRepresentative>();
        }
    }
}
