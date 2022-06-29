using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;

public class CreateSpecialRegisterHandler : ICommandHandler<CreateSpecialRegisterCommand, ResultObject>
{
    private readonly IMapper _mapper;
    private readonly ISpecialRegisterService _specialRegisterService;

    public CreateSpecialRegisterHandler(
        IMapper mapper,
        ISpecialRegisterService specialRegisterService)
    {
        _mapper = mapper;
        _specialRegisterService = specialRegisterService;
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

            var isNameDuplicate = await _specialRegisterService
                .AnyAsync(c => c.Name.ToLower() == request.Name.ToLower(),
                    cancellationToken);

            if (isNameDuplicate)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Special Register name is already used"
                });
            }

            var isTypeDuplicate =
                await _specialRegisterService.AnyAsync(c => c.DocumentType == request.DocumentType, cancellationToken);

            if (isTypeDuplicate)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Document Type already has a special register created"
                });
            }

            var newRegister = _mapper.Map<SpecialRegister>(request);

            await _specialRegisterService.CreateAsync(newRegister, cancellationToken);
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