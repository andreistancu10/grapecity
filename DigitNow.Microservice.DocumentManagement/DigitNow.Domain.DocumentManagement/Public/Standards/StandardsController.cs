using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetStandards;
using DigitNow.Domain.DocumentManagement.Public.Standards.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Standards
{
    [Authorize]
    [ApiController]
    [Route("api/standards")]
    public class StandardsController: ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StandardsController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStandardAsync([FromBody] CreateStandardRequest request)
        {
            return await _mediator.Send(_mapper.Map<CreateStandardCommand>(request))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStandardAsync([FromBody] UpdateStandardRequest request)
        {
            var command = _mapper.Map<UpdateStandardCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetStandardByIdAsync([FromRoute] long id)
        {
            return await _mediator.Send(new GetStandardByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost("get-standards")]
        public async Task<IActionResult> GetStandardsAsync([FromBody] GetStandardsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetStandardsQuery>(request);
            return CreateResponse(await _mediator.Send(query, cancellationToken));
        }


    }
}
