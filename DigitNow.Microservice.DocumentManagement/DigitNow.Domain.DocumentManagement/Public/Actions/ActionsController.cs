﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands;
using DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.Actions.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Actions
{
    [AllowAnonymous]
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

        [HttpPut()]
        public async Task<IActionResult> UpdateActionAsync([FromBody] UpdateActionRequest request)
        {
            var command = _mapper.Map<UpdateActionCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetActionByIdAsync([FromRoute] long id)
        {
            return await _mediator.Send(new GetActionByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}
