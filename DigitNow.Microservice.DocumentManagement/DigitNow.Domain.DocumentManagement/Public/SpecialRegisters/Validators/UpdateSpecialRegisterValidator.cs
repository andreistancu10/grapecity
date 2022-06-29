using DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Validators;

public class UpdateSpecialRegisterValidator : AbstractValidator<UpdateSpecialRegisterRequest>
{
    public UpdateSpecialRegisterValidator()
    {
        RuleFor(item => item.DocumentCategoryId).NotNull().NotEmpty();
    }
}