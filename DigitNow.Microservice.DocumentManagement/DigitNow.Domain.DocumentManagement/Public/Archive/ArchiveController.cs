using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Archive.Commands;
using DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.RemoveHistoricalArchiveDocument;
using DigitNow.Domain.DocumentManagement.Business.Archive.Queries;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms;
using DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchive;
using DigitNow.Domain.DocumentManagement.Business.OperationalArchive.Commands.Update.MoveDocumentsToArchiveByIds;
using DigitNow.Domain.DocumentManagement.Public.Archive.Models;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using DigitNow.Domain.DocumentManagement.Public.DynamicForms.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Archive
{
    [Authorize]
    [ApiController]
    [Route("api/archive")]
    public class ArchiveController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ArchiveController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("operational/get-documents")]
        public async Task<IActionResult> GetOperationalArchiveDocumentsAsync([FromBody] GetDocumentsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetDocumentsOperationalArchiveQuery>(request);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPut("operational/delete")]
        public async Task<IActionResult> RemoveDocument([FromBody] RemoveDocumentRequest request, CancellationToken cancellationToken)
        {
            var deleteDocumentCommand = _mapper.Map<RemoveDocumentCommand>(request);
            return CreateResponse(await _mediator.Send(deleteDocumentCommand, cancellationToken));
        }

        [HttpPost("historical/get-documents")]
        public async Task<IActionResult> GetHistoricalArchiveDocumentsAsync([FromBody] GetHistoricalArchiveDocumentsRequest request, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetHistoricalArchiveDocumentsQuery>(request);
            return CreateResponse(await _mediator.Send(query, cancellationToken));
        }

        [HttpDelete("historical/delete/{id}")]
        public async Task<IActionResult> RemoveHistoricalArchiveDocumentAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            var request = new RemoveHistoricalArchiveDocumentCommand { Id = id };

            var deleteDocumentCommand = _mapper.Map<RemoveHistoricalArchiveDocumentCommand>(request);

            return CreateResponse(await _mediator.Send(deleteDocumentCommand, cancellationToken));
        }

        [HttpPut("operational/job")]
        public async Task<IActionResult> MoveDocumentsToArchiveAsync()
        {
            return await _mediator.Send(new MoveDocumentsToArchiveCommand())
           switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpPut("operational/byIds")]
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