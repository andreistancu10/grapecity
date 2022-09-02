using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands;
using DigitNow.Domain.DocumentManagement.Public.Actions.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Actions
{
    [Authorize]
    [ApiController]
    [Route("api/actions")]
    public class ActionsController: ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ActionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateActionAsync([FromBody] CreateActionRequest request)
        {
            return await _mediator.Send(_mapper.Map<CreateActionCommand>(request))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}
