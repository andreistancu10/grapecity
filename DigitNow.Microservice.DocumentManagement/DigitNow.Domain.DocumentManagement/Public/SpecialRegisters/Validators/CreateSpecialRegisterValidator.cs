using DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Validators;

public class CreateSpecialRegisterValidator : AbstractValidator<CreateSpecialRegisterRequest>
{
    public CreateSpecialRegisterValidator()
    {
        RuleFor(item => item.DocumentCategoryId).NotNull().NotEmpty();
        RuleFor(item => item.Name).NotNull().NotEmpty();
    }
}