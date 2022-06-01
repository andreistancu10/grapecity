using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.GetList
{
    internal sealed class GetNotificationTypesHandler : IQueryHandler<GetNotificationTypesQuery, IList<GetNotificationTypesResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, GetNotificationTypesResponse>();
        });
        
        public GetNotificationTypesHandler(DocumentManagementDbContext dbContext)
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