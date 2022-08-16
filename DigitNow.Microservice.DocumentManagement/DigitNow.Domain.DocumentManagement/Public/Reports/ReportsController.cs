using System;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Public.Reports.Models;
using DigitNow.Domain.DocumentManagement.utils;
using Domain.Localization.Client;
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
        private readonly IExportService<ExportReportViewModel> _exportService;
        private readonly ILocalizationManager _localizationManger;

        public ReportsController(
            IMediator mediator,
            IMapper mapper,
            IExportService<ExportReportViewModel> exportService,
            ILocalizationManager localizationManger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _exportService = exportService;
            _localizationManger = localizationManger;
        }

        [HttpPost("expired")]
        public async Task<IActionResult> GetExpiredDocumentsReportAsync([FromBody] GetExpiredReportRequest request, CancellationToken cancellationToken)
        {
            var getReportQuery = _mapper.Map<GetReportQuery>(request);
            var reportResult = await _mediator.Send(getReportQuery, cancellationToken);
            if (reportResult == null) return NotFound();

            var documentNameResponse = await _localizationManger.Translate(request.LanguageId, "dms.reports.expired-document-name", cancellationToken);
            var documentFileName = $"{documentNameResponse.Translation} {request.FromDate.ToString("dd-MM-yyyy")} - {request.ToDate.ToString("dd-MM-yyyy")}";

            var fileResult = await _exportService.CreateExcelFile(documentFileName, documentNameResponse.Translation, reportResult);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpPost("to-expire")]
        public async Task<IActionResult> GetAboutToExpireDocumentsReportAsync([FromBody] GetToExpireReportRequest request, CancellationToken cancellationToken)
        {
            var getReportQuery = _mapper.Map<GetReportQuery>(request);
            var reportResult = await _mediator.Send(getReportQuery, cancellationToken);
            if (reportResult == null) return NotFound();

            var documentNameResponse = await _localizationManger.Translate(request.LanguageId, "dms.reports.to-expire-document-name", cancellationToken);
            var documentFileName = $"{documentNameResponse.Translation} {request.FromDate.ToString("dd-MM-yyyy")} - {request.ToDate.ToString("dd-MM-yyyy")}";

            var fileResult = await _exportService.CreateExcelFile(documentFileName, documentNameResponse.Translation, reportResult);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }
    }
}