using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.GetById
{
    internal sealed class GetNotificationStatusByIdHandler : IQueryHandler<GetNotificationStatusByIdQuery, GetNotificationStatusByIdResponse>
    {
        private readonly TenantNotificationDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationStatus, GetNotificationStatusByIdResponse>();
        });
        
        public GetNotificationStatusByIdHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetNotificationStatusByIdResponse> Handle(GetNotificationStatusByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.NotificationStatuses
                .ProjectTo<GetNotificationStatusByIdResponse>(_mappingConfiguration)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}