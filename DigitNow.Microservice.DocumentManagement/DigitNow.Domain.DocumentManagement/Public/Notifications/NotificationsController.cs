using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.ApproveNotification;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.CancelNotification;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.ChangeSeen;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.ChangeStatus;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.RejectNotification;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetList;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetUnseenCountByUserId;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Helpers;
using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.Files;
using HTSS.Platform.Infrastructure.Api.Tools;
using HTSS.Platform.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ApiController
    {
        private readonly IExportService<FilterNotificationResponse> _exportService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public NotificationsController(IMediator mediator,
            IMapper mapper,
            IExportService<FilterNotificationResponse> exportService, IIdentityService identityService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _exportService = exportService;
            _identityService = identityService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetNotificationByIdQuery
                {
                    Id = id
                }, cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] FilterNotificationsRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(_mapper.Map<FilterNotificationQuery>(request), cancellationToken));
        }

        [HttpGet("currentuser/unseen/count")]
        public async Task<IActionResult> GetCurrentUserUnseenCount(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetUnseenCountByUserIdQuery(), cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [HttpGet("currentuser/filter")]
        public async Task<IActionResult> GetCurrentUserFiltered([FromQuery] FilterNotificationsRequest model, CancellationToken cancellationToken)
        {
            var currentUser = _identityService.AuthenticatedUser;
            model.UserId = currentUser.UserId;

            if (model.NotificationTypeCategoryIds != null && model.NotificationTypeCategoryIds.Count > 0 &&
                !model.IsInformative.HasValue)
            {
                if (model.NotificationTypeCategoryIds.Count == 1 &&
                    model.NotificationTypeCategoryIds.Contains((long) NotificationTypeCategoryEnum.Informative))
                    model.IsInformative = true;

                if (model.NotificationTypeCategoryIds.Count == 1 &&
                    model.NotificationTypeCategoryIds.Contains((long) NotificationTypeCategoryEnum.Reactive))
                    model.IsInformative = false;

                if (model.NotificationTypeCategoryIds.Contains((long) NotificationTypeCategoryEnum.Reactive) &&
                    model.NotificationTypeCategoryIds.Contains((long) NotificationTypeCategoryEnum.Informative))
                    model.IsInformative = null;
            }
            
            return Ok(await _mediator.Send(_mapper.Map<FilterNotificationQuery>(model), cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map<CreateNotificationCommand>(request), cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification([FromRoute] long id, [FromBody] UpdateNotificationRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map(request, new UpdateNotificationCommand 
            {
                Id = id
            }), cancellationToken));
        }

        [HttpPut("{id}/seenflag")]
        public async Task<IActionResult> ChangeSeen([FromRoute] long id, [FromBody] UpdateNotificationSeenFlagRequest model, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new ChangeNotificationSeenFlagCommand
            {
                Id = id,
                Seen = model.Seen
            }, cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpPut("{id}/changestatus")]
        public async Task<IActionResult> ChangeStatus([FromRoute] long id, [FromBody] UpdateNotificationStatusRequest model, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new ChangeNotificationStatusCommand
            {
                Id = id,
                NotificationStatusId = model.StatusId
            }, cancellationToken));
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveNotification([FromRoute] long id, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new ApproveNotificationCommand
            {
                Id = id
            }, cancellationToken));
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectNotification([FromRoute] long id, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new RejectNotificationCommand
            {
                Id = id
            }, cancellationToken));
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelNotification(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new CancelNotificationCommand
            {
                Id = id
            }, cancellationToken));
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpGet("list")]
        public async Task<IActionResult> GetNotifications(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetNotificationsQuery(), cancellationToken) switch
            {
                null => NotFound(),
                var result => Ok(result)
            };
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpGet("export/excel")]
        public async Task<IActionResult> GetExcel([FromQuery] FilterNotificationsRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateExcelFile("Notifications", "Notifications", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpGet("export/csv")]
        public async Task<IActionResult> GetCsv([FromQuery] FilterNotificationsRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateCsvFile("Notifications", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpGet("export/json")]
        public async Task<IActionResult> GetJson([FromQuery] FilterNotificationsRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationQuery>(request), cancellationToken);
            var fileResult = await _exportService.CreateJsonFile("Notifications", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }

        [TypeFilter(typeof(DevOnlyActionFilter))]
        [HttpGet("export/xml")]
        public async Task<IActionResult> GetXml([FromQuery] FilterNotificationsRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(_mapper.Map<FilterNotificationQuery>(request), cancellationToken);
            var fileResult = _exportService.CreateXmlFile("Notifications", result.Items);

            return File(fileResult.Content, fileResult.ContentType, fileResult.Name);
        }
    }
}