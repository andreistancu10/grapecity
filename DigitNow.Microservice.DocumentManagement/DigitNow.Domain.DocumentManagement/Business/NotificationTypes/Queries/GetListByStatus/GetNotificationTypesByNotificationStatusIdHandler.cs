using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetListByStatus
{
    internal sealed class GetNotificationTypesByNotificationStatusIdHandler : IQueryHandler<GetNotificationTypesByNotificationStatusIdQuery, IList<GetNotificationTypesByNotificationStatusIdResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, GetNotificationTypesByNotificationStatusIdResponse>()
                .ForMember(dest => dest.NotificationStatusName, 
                    opt => opt.MapFrom(src => src.NotificationStatus.Name));

        });

        public GetNotificationTypesByNotificationStatusIdHandler(DocumentManagementDbContext dbContext)
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