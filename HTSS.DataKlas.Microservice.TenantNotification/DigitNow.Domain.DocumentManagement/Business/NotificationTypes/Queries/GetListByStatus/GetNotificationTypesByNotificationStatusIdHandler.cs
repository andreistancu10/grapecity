using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetListByStatus
{
    internal sealed class GetNotificationTypesByNotificationStatusIdHandler : IQueryHandler<GetNotificationTypesByNotificationStatusIdQuery, IList<GetNotificationTypesByNotificationStatusIdResponse>>
    {
        private readonly TenantNotificationDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, GetNotificationTypesByNotificationStatusIdResponse>()
                .ForMember(dest => dest.NotificationStatusName, 
                    opt => opt.MapFrom(src => src.NotificationStatus.Name));

        });

        public GetNotificationTypesByNotificationStatusIdHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<GetNotificationTypesByNotificationStatusIdResponse>> Handle(GetNotificationTypesByNotificationStatusIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.NotificationTypes
                .Include(p => p.NotificationStatus)
                .Where(p => p.NotificationStatusId == request.NotificationStatusId)
                .ProjectTo<GetNotificationTypesByNotificationStatusIdResponse>(_mappingConfiguration)
                .ToListAsync(cancellationToken);

            if (result.Any())
            {
                result.ForEach(item =>
                {
                    item.EntityTypeName = Enum.GetName(typeof(NotificationEntityTypeEnum), item.EntityType);
                });
            }
            
            return result;
        }
    }
}