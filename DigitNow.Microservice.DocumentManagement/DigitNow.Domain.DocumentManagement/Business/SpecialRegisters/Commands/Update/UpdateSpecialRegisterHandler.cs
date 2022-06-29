using System;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update;

public class UpdateSpecialRegisterHandler : ICommandHandler<UpdateSpecialRegisterCommand, ResultObject>
{
    private readonly ISpecialRegisterService _specialRegisterService;

    public UpdateSpecialRegisterHandler(
        ISpecialRegisterService specialRegisterService)
    {
        _specialRegisterService = specialRegisterService;
    }

    public async Task<ResultObject> Handle(UpdateSpecialRegisterCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.IsDefined(typeof(DocumentType), command.DocumentType))
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Document Type does not exist",
                    Parameters = new[] { nameof(command.DocumentType) }
                });
            }

            var documentTypeAlreadyHasRegister =
                await _specialRegisterService.AnyAsync(c => c.DocumentType == command.DocumentType, cancellationToken);

            if (documentTypeAlreadyHasRegister)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Document Type already has a special register created"
                });
            }

            var specialRegister = await _specialRegisterService
                .FindAsync(c => c.Id == command.Id,
                    cancellationToken);

            if (specialRegister == null)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Special Register not found"
                });
            }

            specialRegister.DocumentType = command.DocumentType;
            await _specialRegisterService.UpdateAsync(specialRegister, cancellationToken);
        }
        catch (Exception e)
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = "Special Register update failed."
            });
        }

        return ResultObject.Ok();
    }
}