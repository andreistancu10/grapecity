using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.GetById
{
    internal sealed class GetNotificationStatusByIdHandler : IQueryHandler<GetNotificationStatusByIdQuery, GetNotificationStatusByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;
        
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationStatus, GetNotificationStatusByIdResponse>();
        });
        
        public GetNotificationStatusByIdHandler(DocumentManagementDbContext dbContext)
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