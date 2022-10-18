using DigitNow.Domain.DocumentManagement.Public.Suppliers.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Suppliers.Validators
{
    public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
    {
        public CreateSupplierRequestValidator()
        {
            RuleFor(item => item.Name).NotNull().NotEmpty();
            RuleFor(item => item.CertificateRegistration).NotNull().NotEmpty();
            RuleFor(item => item.VatPayer).NotNull();
            RuleFor(item => item.CompanyType).NotNull().NotEmpty();
            RuleFor(item => item.RegisteredOfficeContactDetail).NotNull().NotEmpty();
            RuleFor(item => item.RegisteredWorkplace).NotNull();
            RuleFor(item => item.LegalRepresentatives).NotNull().NotEmpty();

        }
    }
}
