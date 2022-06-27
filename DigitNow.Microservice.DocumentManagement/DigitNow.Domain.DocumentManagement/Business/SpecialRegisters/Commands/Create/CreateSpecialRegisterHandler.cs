using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.SpecialRegisters;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;

public class CreateSpecialRegisterHandler : ICommandHandler<CreateSpecialRegisterCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateSpecialRegisterHandler(DocumentManagementDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultObject> Handle(CreateSpecialRegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.IsDefined(typeof(DocumentType), request.DocumentType))
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Document Type does not exist",
                    Parameters = new[] { nameof(request.DocumentType) }
                });
            }

            var isDuplicate = await _dbContext.SpecialRegisters
                .AnyAsync(c =>
                        c.Name.ToLower() == request.Name.ToLower() || c.DocumentType == request.DocumentType,
                    cancellationToken);

            if (isDuplicate)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Special Register name is already used"
                });
            }

            var documentTypeAlreadyHasRegister =
                await _dbContext.SpecialRegisters.AnyAsync(c => c.DocumentType == request.DocumentType, cancellationToken);

            if (documentTypeAlreadyHasRegister)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Document Type already has a special register created"
                });
            }

            var newRegister = _mapper.Map<SpecialRegister>(request);
            newRegister.CreationDate = DateTime.UtcNow;

            await _dbContext.SpecialRegisters.AddAsync(newRegister, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = "Special Register creation failed."
            });
        }

        return ResultObject.Ok();
    }
}