using System.Collections.Generic;
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
using HTSS.Platform.Core.Files;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard;

[Authorize]
[Route("api/dashboard")]
public class DashboardController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IExportService<GetDocumentResponse> _exportService;

    public DashboardController(IMediator mediator, IMapper mapper, IExportService<GetDocumentResponse> exportService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _exportService = exportService;
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

    [HttpGet("get-documents")]
    public async Task<ActionResult<List<GetDocumentResponse>>> GetDocuments([FromQuery] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetDocumentsQuery>(request);
        return Ok(await _mediator.Send(command, cancellationToken));
    }


    [HttpGet("export-excel")]
    public async Task<ActionResult<FileContentResult>> ExportExcel([FromQuery] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetDocumentsQuery>(request);
        var result = await _mediator.Send(command, cancellationToken);
        var fileResult = await _exportService.CreateExcelFile("Permissions", "Permissions", result.Items);

        return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
    }
}