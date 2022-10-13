using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HTSS.Platform.Infrastructure.Api.Tools;
using DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions.Models;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Update;

namespace DigitNow.Domain.DocumentManagement.Public.PublicAcquisitions
{
    [Authorize]
    [ApiController]
    [Route("api/public-acquisitions")]
    public class PublicAcquisitionController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PublicAcquisitionController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return await _mediator.Send(new GetPublicAcquisitionProjectByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePublicAcquisitionProjectRequest request)
        {
            var command = _mapper.Map<CreatePublicAcquisitionProjectCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePublicAcquisitionProjectRequest request)
        {
            var command = _mapper.Map<UpdatePublicAcquisitionProjectCommand>(request);

            return CreateResponse(await _mediator.Send(command));
        }
    }
}
