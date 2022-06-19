using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.UpdateUserRecipient;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard;

[Authorize]
[Route("api/dashboard")]
public class DashboardController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DashboardController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPut("update-department")]
    public async Task<IActionResult> UpdateDocumentRecipientByDepartmentId([FromBody] UpdateDocumentRecipientRequest request, CancellationToken cancellationToken)
    {
        var updateDepartmentForDocumentCommand = _mapper.Map<UpdateDocumentRecipientCommand>(request);

        return CreateResponse(await _mediator.Send(updateDepartmentForDocumentCommand, cancellationToken));
    }

    [HttpPut("update-user-recipient")]
    public async Task<IActionResult> UpdateDocumentRecipientByUserId([FromBody] UpdateDocumentUserRecipientRequest request, CancellationToken cancellationToken)
    {
        var updateUserRecipientForDocumentCommand = _mapper.Map<UpdateDocumentUserRecipientCommand>(request);
        return CreateResponse(await _mediator.Send(updateUserRecipientForDocumentCommand, cancellationToken));
    }

    [HttpPut("get-documents")]
    public async Task<IActionResult> GetDocuments([FromBody] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetDocumentsQuery>(request);
        return CreateResponse(await _mediator.Send(command, cancellationToken));
    }
}