using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRisk;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRiskTrackingReport;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRisks;
using DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRiskTrackingReports;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;
using DigitNow.Domain.DocumentManagement.Public.Risks.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Risks
{
    [Authorize]
    [ApiController]
    [Route("api/risk")]
    public class RiskController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RiskController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return await _mediator.Send(new GetRiskByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRiskRequest request)
        {
            var command = _mapper.Map<CreateRiskCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRiskRequest request)
        {
            var command = _mapper.Map<UpdateRiskCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("calculateRiskExposure/{probability}/{impact}")]
        public IActionResult CalculateRiskExposureEvaluation([FromRoute] RiskProbability probability, [FromRoute] RiskProbability impact)
        {
            if (probability != 0 && impact != 0)
                return Ok(new
                {
                    RiskExposure = RiskService.CalculateRiskExposureEvaluation(probability, impact)
                });
            return BadRequest();
        }

        [HttpPost("get-risks")]
        public async Task<IActionResult> GetRisksAsync([FromBody] GetRisksRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetRisksQuery>(request);
            return CreateResponse(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost("{id}/tracking-report")]
        public async Task<IActionResult> CreateRiskTrackingReport([FromRoute] int id, [FromBody] CreateRiskTrackingReportRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateRiskTrackingReportCommand>(request);
            command.RiskId = id;

            return CreateResponse(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{id}/tracking-reports")]
        public async Task<IActionResult> GetRiskTrackingReports([FromRoute] int id, [FromBody] GetRiskTrackingReportsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetRiskTrackingReportsQuery>(request);
            query.RiskId = id;

            return CreateResponse(await _mediator.Send(query, cancellationToken));
        }
    }
}
