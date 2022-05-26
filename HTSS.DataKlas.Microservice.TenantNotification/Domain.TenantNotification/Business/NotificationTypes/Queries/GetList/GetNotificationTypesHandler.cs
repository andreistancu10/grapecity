using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetList
{
    internal sealed class GetNotificationTypesHandler : IQueryHandler<GetNotificationTypesQuery, IList<GetNotificationTypesResponse>>
    {
        private readonly TenantNotificationDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, GetNotificationTypesResponse>();
        });
        
        public GetNotificationTypesHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<GetNotificationTypesResponse>> Handle(GetNotificationTypesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.NotificationTypes
                .ProjectTo<GetNotificationTypesResponse>(_mappingConfiguration)
                .ToListAsync(cancellationToken);
        }
    }
}