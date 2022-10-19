using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Commands;
using DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.PerformanceIndicators.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.PerformanceIndicators
{
    [Authorize]
    [ApiController]
    [Route("api/performance-indicators")]
    public class PerformanceIndicatorsController: ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PerformanceIndicatorsController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePerformanceIndicatorAsync([FromBody] CreatePerformanceIndicatorRequest request)
        {
            return await _mediator.Send(_mapper.Map<CreatePerformanceIndicatorCommand>(request))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStandardAsync([FromBody] UpdatePerformanceIndicatorRequest request)
        {
            var command = _mapper.Map<UpdatePerformanceIndicatorCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetStandardByIdAsync([FromRoute] long id)
        {
            return await _mediator.Send(new GetPerformanceIndicatorByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}
