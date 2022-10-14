using DigitNow.Domain.DocumentManagement.Public.Suppliers.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Suppliers.Validators
{
    public class UpdateSupplierRequestValidator : AbstractValidator<UpdateSupplierRequest>
    {
        public UpdateSupplierRequestValidator()
        {
            RuleFor(item => item.Name).NotNull().NotEmpty();
            RuleFor(item => item.CertificateRegistration).NotNull().NotEmpty();
            RuleFor(item => item.VatPayer).NotNull().NotEmpty();
            RuleFor(item => item.Status).NotNull();
            RuleFor(item => item.CompanyType).NotNull().NotEmpty();
            RuleFor(item => item.RegisteredOfficeContactDetail).NotNull().NotEmpty();
            RuleFor(item => item.RegisteredWorkplace).NotNull().NotEmpty();
            RuleFor(item => item.LegalRepresentatives).NotNull().NotEmpty();

        }
    }
}
