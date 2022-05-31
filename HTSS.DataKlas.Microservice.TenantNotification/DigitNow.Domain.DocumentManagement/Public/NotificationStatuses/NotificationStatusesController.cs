using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Create;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Update;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.GetById;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.GetList;
using ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models;
using ShiftIn.Domain.TenantNotification.utils;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses
{
    [Authorize]
    [ApiController]
    [Route("api/notificationStatuses")]
    public class NotificationStatusesController : ApiController
    {
        private readonly IExportService<FilterNotificationStatusResponse> _exportService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public NotificationStatusesController(IMediator mediator,
            IMapper mapper,
            IExportService<FilterNotificationStatusResponse> exportService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _exportService = exportService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetNotificationStatusByIdQuery
                {
                    Id = id
                }, cancellationToken) switch
            {
                null => NotFound(),
                var district => Ok(district)
            };
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] FilterNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<FilterNotificationStatusQuery>(request), cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateNotificationStatus([FromBody] CreateNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map<CreateNotificationStatusCommand>(request), cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotificationStatus([FromRoute] long id, [FromBody] UpdateNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map(request, new UpdateNotificationStatusCommand 
            {
                Id = id
            }), cancellationToken));
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetNotificationStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetNotificationStatusesQuery(), cancellationToken);

            return (result is null) switch
            {
                true => NotFound(),
                _ => Ok(result)
            };
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> GetExcel([FromQuery] FilterNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationStatusQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateExcelFile("NotificationStatuses", "NotificationStatuses", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> GetCsv([FromQuery] FilterNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationStatusQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateCsvFile("NotificationStatuses", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpGet("export/json")]
        public async Task<IActionResult> GetJson([FromQuery] FilterNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationStatusQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateJsonFile("NotificationStatuses", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpGet("export/xml")]
        public async Task<IActionResult> GetXml([FromQuery] FilterNotificationStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationStatusQuery>(request), cancellationToken);
            var fileResult = _exportService.CreateXmlFile("NotificationStatuses", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }
    }
}