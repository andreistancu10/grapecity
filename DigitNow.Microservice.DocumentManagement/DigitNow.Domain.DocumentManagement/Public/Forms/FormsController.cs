using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Forms.Commands;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById;
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
        private readonly ILocalizationManager _localizationManger;

        public FormsController(
            IMediator mediator,
            IMapper mapper,
            ILocalizationManager localizationManger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _localizationManger = localizationManger;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAsync([FromQuery] FilterFormsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<FilterFormsQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("getById/{id:long}")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] long id, CancellationToken cancellationToken)
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