using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums;
using ShiftIn.Utils.Helpers;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.GetActionsForNotificationsFlow
{
    public class GetActionsForNotificationsFlowHandler : IQueryHandler<GetActionsForNotificationsFlowQuery, IList<GetActionsForNotificationsFlowResponse>>
    {
        public async Task<IList<GetActionsForNotificationsFlowResponse>> Handle(GetActionsForNotificationsFlowQuery request, CancellationToken cancellationToken)
        {
            var result = Enum.GetValues(typeof(NotificationTypeCoverGapAction))
                                .ToListDynamic()
                                .Select(x => new GetActionsForNotificationsFlowResponse
                                {
                                     Id = (int)x,
                                     Name = EnumDescriptionHelper.Get((NotificationTypeCoverGapAction)x)
                                })
                                .ToList();
            return await Task.FromResult(result);
        }
    }
}
