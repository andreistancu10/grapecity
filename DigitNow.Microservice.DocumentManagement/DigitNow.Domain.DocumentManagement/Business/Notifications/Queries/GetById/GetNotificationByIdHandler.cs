using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetById
{
    internal sealed class GetNotificationByIdHandler : IQueryHandler<GetNotificationByIdQuery, GetNotificationByIdResponse>
    {
        private readonly DocumentManagementDbContext _dbContext;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Notification, GetNotificationByIdResponse>();
        });
        
        public GetNotificationByIdHandler(DocumentManagementDbContext dbContext)
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