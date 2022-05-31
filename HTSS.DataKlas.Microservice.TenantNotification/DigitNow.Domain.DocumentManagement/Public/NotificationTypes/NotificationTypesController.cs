using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Commands.Create;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Commands.Update;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetById;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetCategories;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetEntityTypes;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetList;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetListByStatus;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models;
using ShiftIn.Domain.TenantNotification.utils;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes
{
    [Authorize]
    [ApiController]
    [Route("api/notificationTypes")]
    public class NotificationTypesController : ApiController
    {
        private readonly IExportService<FilterNotificationTypesResponse> _exportService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public NotificationTypesController(IMediator mediator,
            IMapper mapper,
            IExportService<FilterNotificationTypesResponse> exportService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _exportService = exportService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetNotificationTypeByIdQuery
                {
                    Id = id
                }, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("notificationStatusId/{notificationStatusId}")]
        public async Task<IActionResult> GetByStatusId(long notificationStatusId, CancellationToken cancellationToken)
        {
            return await _mediator.Send(
                    new GetNotificationTypesByNotificationStatusIdQuery {NotificationStatusId = notificationStatusId},
                    cancellationToken) switch
                {
                    null => NotFound(),
                    var result => Ok(result)
                };
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] FilterNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<FilterNotificationTypesQuery>(request), cancellationToken));
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetNotificationTypes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetNotificationTypesQuery(), cancellationToken);

            return (result is null) switch
            {
                true => NotFound(),
                _ => Ok(result)
            };
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetNotificationTypeCategories(CancellationToken cancellationToken)
        {
            return await _mediator.Send(
                    new GetCategoriesQuery(),
                    cancellationToken) switch
                {
                    null => NotFound(),
                    var result => Ok(result)
                };
        }

        [HttpGet("entityTypes")]
        public async Task<IActionResult> GetNotificationEntityTypes(CancellationToken cancellationToken)
        {
            return await _mediator.Send(
                    new GetEntityTypesQuery(),
                    cancellationToken) switch
                {
                    null => NotFound(),
                    var result => Ok(result)
                };
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateNotificationType([FromBody] CreateNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map<CreateNotificationTypeCommand>(request), cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotificationType([FromRoute] long id, [FromBody] UpdateNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map(request, new UpdateNotificationTypeCommand 
            {
                Id = id
            }), cancellationToken));
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> GetExcel([FromQuery] FilterNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationTypesQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateExcelFile("NotificationTypes", "NotificationTypes", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> GetCsv([FromQuery] FilterNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationTypesQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateCsvFile("NotificationTypes", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpGet("export/json")]
        public async Task<IActionResult> GetJson([FromQuery] FilterNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationTypesQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateJsonFile("NotificationTypes", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [HttpGet("export/xml")]
        public async Task<IActionResult> GetXml([FromQuery] FilterNotificationTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationTypesQuery>(request), cancellationToken);
            var fileResult = _exportService.CreateXmlFile("NotificationTypes", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }
    }
}