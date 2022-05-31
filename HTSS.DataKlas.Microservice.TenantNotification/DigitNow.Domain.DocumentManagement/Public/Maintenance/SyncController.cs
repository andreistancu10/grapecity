using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Sync;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Sync;

namespace ShiftIn.Domain.TenantNotification.Public.Maintenance
{
    [Authorize]
    [ApiController]
    [Route("api/sync")]
    public class SyncController : ApiController
    {
        private readonly IMediator _mediator;

        public SyncController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("synchronize/notifications")]
        public async Task<IActionResult> ExecuteSynchronizeNotifications(CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new SyncNotificationsCommand(), cancellationToken));
        }

        [HttpPut("synchronize/notification-type-cover-gap-extension")]
        public async Task<IActionResult> ExecuteSynchronizeNotificationTypeCoverGapExtensions(CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(new SyncNotificationTypeCoverGapExtensionCommand(), cancellationToken));
        }
    }
}