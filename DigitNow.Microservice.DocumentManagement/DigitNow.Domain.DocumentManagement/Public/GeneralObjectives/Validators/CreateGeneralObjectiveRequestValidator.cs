using DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Validators
{
    public class CreateGeneralObjectiveRequestValidator : AbstractValidator<CreateGeneralObjectiveRequest>
    {
        public CreateGeneralObjectiveRequestValidator()
        {
            RuleFor(item => item.Title).NotNull().NotEmpty();
            RuleFor(item => item.Details).NotNull().NotEmpty();
        }
    }
}