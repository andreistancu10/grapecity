using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities;
using DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives;
using DigitNow.Domain.DocumentManagement.Public.Activities.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Activities
{
    [Authorize]
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ActivitiesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpPost("filter")]
        public async Task<IActionResult> GetGeneralObjectivesAsync([FromBody] FilterActivitiesRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<FilterActivitiesQuery>(request);
            return Ok(await _mediator.Send(query, cancellationToken));
        }
    }
}
