using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchive;
using DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchiveByIds;
using DigitNow.Domain.DocumentManagement.Public.OperationalArchive.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.OperationalArchive
{
    [Authorize]
    [ApiController]
    [Route("api/operational-archive")]
    public class OperationalArchiveController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OperationalArchiveController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpPut]
        public async Task<IActionResult> MoveDocumentsToArchiveAsync()
        {
            return await _mediator.Send(new MoveDocumentsToArchiveCommand())
           switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPut("byIds")]
       public async Task<IActionResult> MoveDocumentsToArchiveByIdsAsync([FromBody] MoveDocumentsToArchiveByIdsRequest request)
        {
            var command = _mapper.Map<MoveDocumentsToArchiveByIdsCommand>(request);
            return await _mediator.Send(command)
           switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

    }
}
