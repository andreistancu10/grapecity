﻿using DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Validators
{
    public class CreateSpecificObjectiveRequestValidator : AbstractValidator<CreateSpecificObjectiveRequest>
    {
        public CreateSpecificObjectiveRequestValidator()
        {
            RuleFor(item => item.GeneralObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.Title).NotNull().NotEmpty();
            RuleFor(item => item.Details).NotNull().NotEmpty();
            RuleFor(item => item.DepartmentId).NotNull().NotEmpty();
            RuleFor(item => item.SpecificObjectiveFunctionaryIds).NotNull().NotEmpty();
        }
    }
}