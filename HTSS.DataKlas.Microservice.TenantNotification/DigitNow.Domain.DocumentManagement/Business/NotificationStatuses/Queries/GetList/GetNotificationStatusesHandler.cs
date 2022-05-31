using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.GetList
{
    internal sealed class GetNotificationStatusesHandler : IQueryHandler<GetNotificationStatusesQuery, IList<GetNotificationStatusesResponse>>
    {
        private readonly TenantNotificationDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationStatus, GetNotificationStatusesResponse>();
        });
        
        public GetNotificationStatusesHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<GetNotificationStatusesResponse>> Handle(GetNotificationStatusesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.NotificationStatuses
                .ProjectTo<GetNotificationStatusesResponse>(_mappingConfiguration)
                .ToListAsync(cancellationToken);
        }
    }
}