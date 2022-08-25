using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Forms.Commands;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetFormById;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;
using Domain.Localization.Client;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Forms
{
    [Authorize]
    [ApiController]
    [Route("api/forms")]
    public class FormsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FormsController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAsync([FromQuery] FilterFormsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<FilterFormsQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var query = new GetFormByIdQuery
            {
                Id = id
            };

            return await _mediator.Send(query, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> SaveFormDataAsync([FromBody] SaveFormDataRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<SaveFormDataCommand>(request);

            return await _mediator.Send(command, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}