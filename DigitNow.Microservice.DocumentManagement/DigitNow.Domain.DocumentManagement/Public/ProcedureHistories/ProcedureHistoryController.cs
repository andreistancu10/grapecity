using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetFilteredProcedures;
using DigitNow.Domain.DocumentManagement.Public.ProcedureHistories.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.ProcedureHistories
{
    [Authorize]
    [ApiController]
    [Route("api/procedure-history")]
    public class ProcedureHistoryController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProcedureHistoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long id)
        {
            return await _mediator.Send(new GetProcedureHistoryByIdQuery(id))
                switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllProcedureHistoriesAsync([FromBody] GetFilteredProcedureHistoriesRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetFilteredProcedureHistoriesQuery>(request);
            return CreateResponse(await _mediator.Send(query, cancellationToken));
        }
    }
}
