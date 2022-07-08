using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

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
        if (await _specialRegisterService
                .AnyAsync(c => c.Name.ToLower() == request.Name.ToLower(),
                    cancellationToken))
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = "Special Register name is already used"
            });
        }

        var isTypeDuplicate =
            _specialRegisterService.AnyAsync(c => c.DocumentCategoryId == request.DocumentCategoryId, cancellationToken);

        if (await isTypeDuplicate)
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = "Document Type already has a special register created"
            });
        }

        var newRegister = _mapper.Map<SpecialRegister>(request);

        await _specialRegisterService.CreateAsync(newRegister, cancellationToken);

        return ResultObject.Ok();
    }
}