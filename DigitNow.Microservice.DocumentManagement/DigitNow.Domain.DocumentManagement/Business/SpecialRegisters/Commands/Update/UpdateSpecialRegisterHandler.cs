using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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
        if (await _specialRegisterService.AnyAsync(c => c.DocumentCategoryId == command.DocumentCategoryId, cancellationToken))
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = "Document Type already has a special register created"
            });
        }

        var specialRegister = await _specialRegisterService
            .FindAsync(c => c.Id == command.Id,
                cancellationToken);
        
        specialRegister.DocumentCategoryId = command.DocumentCategoryId;
        await _specialRegisterService.UpdateAsync(specialRegister, cancellationToken);

        return ResultObject.Ok();
    }
}