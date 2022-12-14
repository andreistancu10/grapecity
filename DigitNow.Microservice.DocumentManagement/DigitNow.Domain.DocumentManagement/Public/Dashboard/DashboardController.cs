using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Commands.UpdateUserRecipient;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries.OperationalDocumentArchiveExport;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard;

[Authorize]
[ApiController]
[Route("api/dashboard")]
public class DashboardController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IExportService<ExportDocumentViewModel> _exportService;

    public DashboardController(IMediator mediator, IMapper mapper, IExportService<ExportDocumentViewModel> exportService)
    {
        _mediator = mediator;        
        _mapper = mapper;
        _exportService = exportService;
    }

    [HttpPut("update-department")]
    public async Task<IActionResult> UpdateDocumentHeadOfDepartmentByDepartmentIdAsync([FromBody] UpdateDocumentHeadofDepartmentRequest request, CancellationToken cancellationToken)
    {
        var updateHeadOfDepartmentForDocumentCommand = _mapper.Map<UpdateDocumentHeadOfDepartmentCommand>(request);

        return CreateResponse(await _mediator.Send(updateHeadOfDepartmentForDocumentCommand, cancellationToken));
    }

    [HttpPut("update-user-recipient")]
    public async Task<IActionResult> UpdateDocumentRecipientByUserIdAsync([FromBody] UpdateDocumentUserRecipientRequest request, CancellationToken cancellationToken)
    {
        var updateUserRecipientForDocumentCommand = _mapper.Map<UpdateDocumentUserRecipientCommand>(request);
        return CreateResponse(await _mediator.Send(updateUserRecipientForDocumentCommand, cancellationToken));
    }

    [HttpPost("get-documents")]
    public async Task<ActionResult<List<GetDocumentsResponse>>> GetDocumentsAsync([FromBody] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetDocumentsQuery>(request);
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpPost("export-excel")]
    public async Task<IActionResult> ExportExcelAsync([FromBody] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DocumentsExportQuery>(request);
        var result = await _mediator.Send(command, cancellationToken);
        var fileResult = await _exportService.CreateExcelFile("Documents", "DocumentsSheet", result);

        return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
    }

    [HttpPost("export-excel-operational-archive")]
    public async Task<IActionResult> ExportExcelOperacionalArchiveAsync([FromBody] GetDocumentsRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<OperationalDocumentArchiveExportQuery>(request);
        var result = await _mediator.Send(command, cancellationToken);
        var fileResult = await _exportService.CreateExcelFile("Documents", "DocumentsSheet", result);

        return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
    }
}