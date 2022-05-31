using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Security;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Commands.Create
{
    internal sealed class CreateNotificationStatusHandler : ICommandHandler<CreateNotificationStatusCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateNotificationStatusCommand, NotificationStatus>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));
        });
        
        public CreateNotificationStatusHandler(DocumentManagementDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(CreateNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = _mappingConfiguration.CreateMapper().Map(request, new NotificationStatus
            {
                CreatedBy = _identityService.AuthenticatedUser.UserId
            });

            _dbContext.Add(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(entity.Id);
        }
    }
}