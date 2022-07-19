using System;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Public.Reports.Models;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Reports
{
    [Authorize]
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IExportService<ReportViewModel> _exportService;

        public ReportsController(
            IMediator mediator,
            IMapper mapper,
            IExportService<ReportViewModel> exportService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _exportService = exportService;
        }

        [HttpPost("get-report")]
        public async Task<IActionResult> GetReportAsync([FromBody] GetReportRequest request)
        {
            var getReportQuery = _mapper.Map<GetReportQuery>(request);
            var reportResult = await _mediator.Send(getReportQuery);

            if (reportResult == null)
            {
                return NotFound();
            }

            var fileResult = await _exportService.CreateExcelFile(request.Type switch
            {
                ReportType.ExpiredDocuments => "Report_Expired",
                ReportType.DocumentsToExpire => "Report_AboutToExpire",
                _ => throw new ArgumentOutOfRangeException()
            }, "DocumentsSheet", reportResult);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }
    }
}