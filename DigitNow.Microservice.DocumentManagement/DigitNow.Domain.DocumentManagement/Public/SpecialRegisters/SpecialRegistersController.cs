using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;
using DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters;

[Authorize]
[ApiController]
[Route("api/special-registers")]
public class SpecialRegistersController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SpecialRegistersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSpecialRegisterAsync([FromBody] CreateSpecialRegisterRequest request)
    {
        return await _mediator.Send(_mapper.Map<CreateSpecialRegisterCommand>(request))
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }

    [HttpPut("update/{id:long}")]
    public async Task<IActionResult> UpdateSpecialRegisterAsync([FromQuery] long id, [FromBody] UpdateSpecialRegisterRequest request)
    {
        var command = _mapper.Map<UpdateSpecialRegisterCommand>(request);
        command.Id = id;

        return await _mediator.Send(command)
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetSpecialRegistersAsync()
    {
        return await _mediator.Send(new SpecialRegisterQuery())
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }
}