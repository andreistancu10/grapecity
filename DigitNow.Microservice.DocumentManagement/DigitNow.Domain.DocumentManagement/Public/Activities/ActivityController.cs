﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.Activities.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Activities
{
    [Authorize]
    [ApiController]
    [Route("api/activity")]
    public class ActivityController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ActivityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateActivityRequest request)
        {
            var command = _mapper.Map<CreateActivityCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateActivityRequest request)
        {
            var command = _mapper.Map<UpdateActivityCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return await _mediator.Send(new GetActivityByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }
    }
}
