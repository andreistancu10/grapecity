using System;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update;

public class UpdateSpecialRegisterHandler : ICommandHandler<UpdateSpecialRegisterCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;

    public UpdateSpecialRegisterHandler(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
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
                await _dbContext.SpecialRegisters.AnyAsync(c => c.DocumentType == command.DocumentType, cancellationToken);

            if (documentTypeAlreadyHasRegister)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Document Type already has a special register created"
                });
            }

            var specialRegister = await _dbContext.SpecialRegisters
                .FirstOrDefaultAsync(c => c.Id == command.Id,
                    cancellationToken);

            if (specialRegister == null)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Special Register not found"
                });
            }

            specialRegister.DocumentType = command.DocumentType;
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