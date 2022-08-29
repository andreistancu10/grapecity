using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.Get;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetAll;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Models;
using DigitNow.Domain.DocumentManagement.Public.SpecificObjective.Models;
using DigitNow.Domain.DocumentManagement.Public.SpecificObjectives.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialObjective
{
    [Authorize]
    [ApiController]
    [Route("api/specific-objectives")]
    public class SpecificObjectiveController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SpecificObjectiveController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecificObjectiveRequest request)
        {
            var command = _mapper.Map<CreateSpecificObjectiveCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSpecificObjectiveRequest request)
        {
            var command = _mapper.Map<UpdateSpecificObjectiveCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return await _mediator.Send(new GetSpecificObjectiveByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("getAllbyUserDepartment")]
        public async Task<ActionResult<List<GetAllByUserDepartmentSpecificObjectiveResponse>>> GetAlByUserDepartment(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllByUserDepartmentSpecificObjectiveQuery(), cancellationToken)
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost("getSpecificObjectives")]
        public async Task<ActionResult<List<GetSpecificObjectiveResponse>>> GetDocumentsAsync([FromBody] GetSpecificObjectiveRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetSpecificObjectiveQuery>(request);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

    }
}
