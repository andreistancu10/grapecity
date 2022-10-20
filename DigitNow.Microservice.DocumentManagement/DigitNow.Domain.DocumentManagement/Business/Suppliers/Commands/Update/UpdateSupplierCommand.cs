using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using HTSS.Platform.Core.CQRS;


namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Commands.Update
{
    public class UpdateSupplierCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public SupplierStatus Status { get; set; }
        public long CertificateRegistration { get; set; }
        public string CommercialRegistration { get; set; }
        public bool VatPayer { get; set; }
        public SupplierCompanyType CompanyType { get; set; }
        public bool RegisteredWorkplace { get; set; }
        public ContactDetailDto RegisteredOfficeContactDetail { get; set; }
        public ContactDetailDto RegisteredWorkplaceContactDetail { get; set; }
        public List<UpdateSupplierLegalRepresentative> LegalRepresentatives { get; set; }

    }
    public class UpdateSupplierLegalRepresentative
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RepresentativeQuality { get; set; }
        public string NationalId { get; set; }

    }
}
