using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Commands;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Forms
{
    [Authorize]
    [ApiController]
    [Route("api/dynamic-forms")]
    public class DynamicFormsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DynamicFormsController(
            IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAsync([FromQuery] FilterDynamicFormsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<DynamicFilterFormsQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{formId:long}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long formId, CancellationToken cancellationToken)
        {
            var query = new GetDynamicFormByIdQuery { FormId = formId };

            return await _mediator.Send(query, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("{formId:long}/{formFillingLogId:long}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] long formId, [FromRoute] long formFillingLogId, CancellationToken cancellationToken)
        {
            var query = new GetDynamicFormByIdQuery { FormId = formId, FormFillingId = formFillingLogId };

            return await _mediator.Send(query, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> SaveDynamicFormDataAsync([FromBody] SaveDynamicFormDataRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<SaveDynamicFormDataCommand>(request);

            return await _mediator.Send(command, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}