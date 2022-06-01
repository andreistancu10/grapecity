using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.GetActionsForNotificationsFlow;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.GetDataForNotificationFlow;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Models;
using HTSS.Platform.Infrastructure.Api.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions
{
    [Authorize]
    [ApiController]
    [Route("api/notificationTypeCoverGapExtensions")]
    public class NotificationTypeCoverGapExtensionController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public NotificationTypeCoverGapExtensionController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("getDataForNotificationFlow")]
        public async Task<IActionResult> GetDataForNotificationFlow(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetDataForNotificationFlowQuery(), cancellationToken));
        }

        [HttpGet("getActionsForNotificationsFlow")]
        public async Task<IActionResult> GetActionsForNotificationsFlow(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetActionsForNotificationsFlowQuery(), cancellationToken));
        }

        [HttpPut("{id}/{actionId}")]
        public async Task<IActionResult> UpdateNotificationTypeCoverGapExtension([FromRoute] long id, [FromRoute] int actionId, [FromBody] UpdateNotificationTypeCoverGapExtensionsRequest request, CancellationToken cancellationToken)
        {
            return CreateResponse(await _mediator.Send(_mapper.Map(request, new UpdateNotificationTypeCoverGapExtensionCommand 
            {
                ActionId = actionId,
                Id = id
            }), cancellationToken));
        }
    }
}