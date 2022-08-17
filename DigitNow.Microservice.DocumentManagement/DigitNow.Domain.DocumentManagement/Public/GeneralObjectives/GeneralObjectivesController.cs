using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetAll;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives
{
    [Authorize]
    [ApiController]
    [Route("api/general-objectives")]
    public class GeneralObjectivesController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GeneralObjectivesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGeneralObjective([FromBody] CreateGeneralObjectiveRequest request)
        {
            var command = _mapper.Map<CreateGeneralObjectiveCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateGeneralObjective([FromBody] UpdateGeneralObjectiveRequest request)
        {
            var command = _mapper.Map<UpdateGeneralObjectiveCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetGeneralObjectiveByIdAsync([FromRoute] long id)
        {
            return await _mediator.Send(new GetGeneralObjectiveByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("getAllGeneralActiveObjective")]
        public async Task<ActionResult<List<GetAllGeneralActiveObjectiveResponse>>> GetAllGeneralActiveObjective(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllGeneralActiveObjectiveQuery(), cancellationToken)
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}
