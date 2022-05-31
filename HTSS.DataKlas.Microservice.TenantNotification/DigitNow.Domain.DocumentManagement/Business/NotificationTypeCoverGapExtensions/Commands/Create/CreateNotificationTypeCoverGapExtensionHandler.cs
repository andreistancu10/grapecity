using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Security;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Create
{
    internal sealed class CreateNotificationTypeCoverGapExtensionHandler : ICommandHandler<CreateNotificationTypeCoverGapExtensionCommand, ResultObject>
    {
        private readonly TenantNotificationDbContext _dbContext;
        private readonly IIdentityService _identityService;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateNotificationTypeCoverGapExtensionCommand, NotificationTypeCoverGapExtension>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));
        });
        
        public CreateNotificationTypeCoverGapExtensionHandler(TenantNotificationDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(CreateNotificationTypeCoverGapExtensionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mappingConfiguration.CreateMapper().Map(request, new NotificationTypeCoverGapExtension
            {
                CreatedBy = _identityService.AuthenticatedUser.UserId
            });
            
            _dbContext.Add(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(entity.Id);
        }
    }
}