using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetFilteredSuppliers;
using DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers;
using DigitNow.Domain.DocumentManagement.Public.Suppliers.Models;


namespace DigitNow.Domain.DocumentManagement.Public.Suppliers.Mappings
{
    public class GetFilteredSupplierMapping :Profile
    {
        public GetFilteredSupplierMapping()
        {
            CreateMap<GetFilteredSupplierRequest, GetFilteredSuppliersQuery>()
                .ForMember(s => s.Filter, opt => opt.MapFrom(src => src.Filter ?? new SupplierFilterDto()));

            CreateMap<SupplierFilterDto, SupplierFilter>();
            {
                CreateMap<SupplierNameFilterDto, SupplierNameFilter>();
                CreateMap<SupplierCertificateRegistrationFilterDto, SupplierCertificateRegistrationFilter>();
                CreateMap<SupplierStatusFilterDto, SupplierStatusFilter>();
            }
        }
    }
}
