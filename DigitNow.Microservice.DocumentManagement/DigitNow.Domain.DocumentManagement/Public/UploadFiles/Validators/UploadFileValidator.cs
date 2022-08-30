using DigitNow.Domain.DocumentManagement.Public.UploadFiles.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles.Validators
{
    public class UploadFileValidator : AbstractValidator<UploadFileRequest>
    {
        public UploadFileValidator()
        {
            RuleFor(item => item.File).NotNull().NotEmpty();
            RuleFor(item => item.TargetEntity).NotNull();
        }
    }
}