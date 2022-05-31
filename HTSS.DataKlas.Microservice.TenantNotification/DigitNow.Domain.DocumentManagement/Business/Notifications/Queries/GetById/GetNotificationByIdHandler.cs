using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.GetById
{
    internal sealed class GetNotificationByIdHandler : IQueryHandler<GetNotificationByIdQuery, GetNotificationByIdResponse>
    {
        private readonly TenantNotificationDbContext _dbContext;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Notification, GetNotificationByIdResponse>();
        });
        
        public GetNotificationByIdHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetNotificationByIdResponse> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Notifications
                .ProjectTo<GetNotificationByIdResponse>(_mappingConfiguration)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}