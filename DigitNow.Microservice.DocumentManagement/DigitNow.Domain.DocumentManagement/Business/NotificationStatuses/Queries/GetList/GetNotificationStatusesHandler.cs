using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.GetList
{
    internal sealed class GetNotificationStatusesHandler : IQueryHandler<GetNotificationStatusesQuery, IList<GetNotificationStatusesResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationStatus, GetNotificationStatusesResponse>();
        });
        
        public GetNotificationStatusesHandler(DocumentManagementDbContext dbContext)
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