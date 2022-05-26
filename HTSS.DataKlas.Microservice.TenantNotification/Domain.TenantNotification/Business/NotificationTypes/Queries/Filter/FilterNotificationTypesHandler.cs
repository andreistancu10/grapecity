using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;


namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.Filter
{
    internal sealed class FilterNotificationTypesHandler : IQueryHandler<FilterNotificationTypesQuery, ResultPagedList<FilterNotificationTypesResponse>>
    {
        private readonly TenantNotificationDbContext _dbContext;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationType, FilterNotificationTypesResponse>()
                .ForMember(dest => dest.NotificationStatusName, opt => opt.MapFrom(src => src.NotificationStatus.Name));
        });
        
        public FilterNotificationTypesHandler(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultPagedList<FilterNotificationTypesResponse>> Handle(FilterNotificationTypesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.NotificationTypes
                .Include(p => p.NotificationStatus).ProjectTo<FilterNotificationTypesResponse>(_mappingConfiguration);
            
            var filterDescriptor = new FilterDescriptor<FilterNotificationTypesResponse>(query, request.SortField, request.SortOrder);
            
            filterDescriptor.Query(p => p.Id, request.Id, () => request.GetFilterMode(p => p.Id));
            filterDescriptor.Query(p => p.Code, request.Code, () => request.GetFilterMode(p => p.Code));
            filterDescriptor.Query(p => p.Name, request.Name, () => request.GetFilterMode(p => p.Name));
            filterDescriptor.Query(p => p.NotificationStatusName, request.NotificationStatusName, () => request.GetFilterMode(f => f.NotificationStatusName));
            filterDescriptor.Query(p => p.EntityType, request.EntityType, () => request.GetFilterMode(f => f.EntityType));
            filterDescriptor.Query(p => p.TranslationLabel, request.TranslationLabel, () => request.GetFilterMode(f => f.TranslationLabel));
            filterDescriptor.Query(p => p.Expression, request.Expression, () => request.GetFilterMode(f => f.Expression));
            filterDescriptor.Query(p => p.IsInformative, request.IsInformative, () => request.GetFilterMode(f => f.IsInformative));
            filterDescriptor.Query(p => p.IsUrgent, request.IsUrgent, () => request.GetFilterMode(f => f.IsUrgent));
            filterDescriptor.Query(p => p.Active, request.Active, () => request.GetFilterMode(f => f.Active));

            filterDescriptor.Order();

            var resultList = await filterDescriptor.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
            
            resultList.List.ForEach(item =>
            {
                item.EntityTypeName = Enum.GetName(typeof(NotificationEntityTypeEnum), item.EntityType);
            });
            
            return resultList.GetResultPagedList();
        }
    }
}