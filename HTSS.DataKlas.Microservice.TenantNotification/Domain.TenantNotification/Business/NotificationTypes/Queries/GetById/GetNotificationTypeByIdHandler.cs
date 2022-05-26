using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.GetById
{
    internal sealed class GetNotificationTypeByIdHandler : IQueryHandler<GetNotificationTypeByIdQuery, GetNotificationTypeByIdResponse>
    {
        private readonly TenantNotificationDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, GetNotificationTypeByIdResponse>()
                .ForMember(dest => dest.NotificationStatusName, opt => opt.MapFrom(src => src.NotificationStatus.Name));
        });
        
        public GetNotificationTypeByIdHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetNotificationTypeByIdResponse> Handle(GetNotificationTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.NotificationTypes.Include(p => p.NotificationStatus)
                .ProjectTo<GetNotificationTypeByIdResponse>(_mappingConfiguration)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if(result is not null)
                result.EntityTypeName = Enum.GetName(typeof(NotificationEntityTypeEnum), result.EntityType);
            
            return result;
        }
    }
}