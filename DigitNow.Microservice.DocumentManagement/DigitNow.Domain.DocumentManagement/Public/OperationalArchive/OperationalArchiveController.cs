using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.OperationalArchive
{
    [Authorize]
    [ApiController]
    [Route("api/operational-archive")]
    public class OperationalArchiveController : ApiController
    {
        private readonly IMediator _mediator;

        public OperationalArchiveController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
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

    }
}
