using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.GetList
{
    internal sealed class GetNotificationsHandler : IQueryHandler<GetNotificationsQuery, IList<GetNotificationsResponse>>
    {
        private readonly TenantNotificationDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Notification, GetNotificationsResponse>();
        });
        
        public GetNotificationsHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<GetNotificationsResponse>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Notifications
                .ProjectTo<GetNotificationsResponse>(_mappingConfiguration)
                .ToListAsync(cancellationToken);
        }
    }
}