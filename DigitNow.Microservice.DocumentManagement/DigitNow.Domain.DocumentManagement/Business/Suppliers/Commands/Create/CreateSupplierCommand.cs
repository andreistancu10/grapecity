using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Create
{
    public class CreateSupplierCommand : ICommand<ResultObject>
    {
       
        public string Name { get; set; }
        public long CertificateRegistration { get; set; }
        public string CommercialRegistration { get; set; }
        public bool VatPayer { get; set; }
        public SupplierCompanyType CompanyType { get; set; }
        public bool RegisteredWorkplace { get; set; }
        public ContactDetailDto RegisteredOfficeContactDetail { get; set; }
        public ContactDetailDto RegisteredWorkplaceContactDetail { get; set; }
        public List<CreateSupplierLegalRepresentative> LegalRepresentatives { get; set; }

    }
    public class CreateSupplierLegalRepresentative
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RepresentativeQuality { get; set; }
        public string NationalId { get; set; }

    }

}
