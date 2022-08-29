using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives;
using DigitNow.Domain.DocumentManagement.Public.ObjectivesDashboard.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace DigitNow.Domain.DocumentManagement.Public.ObjectivesDashboard
{
    [ApiController]
    //[Authorize]
    [Route("api/objectives-dashboard")]
    public class ObjectivesDashboardController: ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ObjectivesDashboardController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;   
        }


        [HttpPost("get-general-objectives")]
        public async Task<ActionResult<List<GetGeneralObjectivesResponse>>> GetGeneralObjectivesAsync([FromBody] GetGeneralObjectivesRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetGeneralObjectivesQuery>(request);
            return Ok(await _mediator.Send(query, cancellationToken));
        }
    }
}
