using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetById
{
    internal sealed class GetNotificationTypeByIdHandler : IQueryHandler<GetNotificationTypeByIdQuery, GetNotificationTypeByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, GetNotificationTypeByIdResponse>()
                .ForMember(dest => dest.NotificationStatusName, opt => opt.MapFrom(src => src.NotificationStatus.Name));
        });
        
        public GetNotificationTypeByIdHandler(DocumentManagementDbContext dbContext)
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