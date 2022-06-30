using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.UploadFiles;

[Authorize]
[ApiController]
[Route("api/upload-file")]
public class UploadFilesController : ApiController
{
    private readonly IFileUploadService _fileUploadService;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UploadFilesController(IFileUploadService fileUploadService, IMediator mediator, IMapper mapper)
    {
        _fileUploadService = fileUploadService;
        _mediator = mediator;
        _mapper = mapper;
    }


    [HttpPost("upload")]
    public async Task<IActionResult> CreateSpecialRegisterAsync([FromBody] CreateSpecialRegisterRequest request)
    {
        return await _mediator.Send(_mapper.Map<CreateSpecialRegisterCommand>(request))
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }

}