using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.Filter
{
    internal sealed class FilterNotificationStatusHandler : IQueryHandler<FilterNotificationStatusQuery, ResultPagedList<FilterNotificationStatusResponse>>
    {
        private readonly DocumentManagementDbContext _dbContext;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationStatus, FilterNotificationStatusResponse>();
        });

        public FilterNotificationStatusHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultPagedList<FilterNotificationStatusResponse>> Handle(FilterNotificationStatusQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.NotificationStatuses
                .ProjectTo<FilterNotificationStatusResponse>(_mappingConfiguration);
            
            var filterDescriptor = new FilterDescriptor<FilterNotificationStatusResponse>(query, request.SortField, request.SortOrder);
            
            filterDescriptor.Query(p => p.Id, request.Id, () => request.GetFilterMode(p => p.Id));
            filterDescriptor.Query(p => p.Code, request.Code, () => request.GetFilterMode(p => p.Code));
            filterDescriptor.Query(p => p.Name, request.Name, () => request.GetFilterMode(p => p.Name));
            filterDescriptor.Query(p => p.Active, request.Active, () => request.GetFilterMode(f => f.Active));

            filterDescriptor.Order();

            var resultList = await filterDescriptor.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
            return resultList.GetResultPagedList();
        }
    }
}