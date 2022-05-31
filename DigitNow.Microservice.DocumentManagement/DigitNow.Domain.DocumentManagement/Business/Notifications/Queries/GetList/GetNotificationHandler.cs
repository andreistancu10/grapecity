using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetList
{
    internal sealed class GetNotificationsHandler : IQueryHandler<GetNotificationsQuery, IList<GetNotificationsResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Notification, GetNotificationsResponse>();
        });
        
        public GetNotificationsHandler(DocumentManagementDbContext dbContext)
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