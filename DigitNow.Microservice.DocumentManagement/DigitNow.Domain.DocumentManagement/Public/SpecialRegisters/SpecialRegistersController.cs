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
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters;

[Authorize]
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

    [HttpPost]
    public async Task<IActionResult> CreateSpecialRegister([FromBody] CreateSpecialRegisterRequest request)
    {
        return await _mediator.Send(_mapper.Map<CreateSpecialRegisterCommand>(request))
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSpecialRegister([FromBody] UpdateSpecialRegisterRequest request)
    {
        return await _mediator.Send(_mapper.Map<UpdateSpecialRegisterCommand>(request))
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }

    [HttpGet]
    public async Task<IActionResult> GetSpecialRegisters()
    {
        return await _mediator.Send(new SpecialRegisterQuery())
            switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
    }
}