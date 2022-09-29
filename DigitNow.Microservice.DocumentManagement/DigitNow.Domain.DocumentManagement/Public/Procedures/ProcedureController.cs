using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.Procedures.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Procedures
{
    [Authorize]
    [ApiController]
    [Route("api/procedure")]
    public class ProcedureController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProcedureController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return await _mediator.Send(new GetProcedureByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProcedureRequest request)
        {
            var command = _mapper.Map<CreateProcedureCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProcedureRequest request)
        {
            var command = _mapper.Map<UpdateProcedureCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }
    }
}
