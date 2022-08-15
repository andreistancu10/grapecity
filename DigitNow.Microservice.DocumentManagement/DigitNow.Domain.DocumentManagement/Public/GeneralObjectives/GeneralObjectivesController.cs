using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives
{
    //[Authorize]
    [AllowAnonymous]
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

    }
}
