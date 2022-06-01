using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Security;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Commands.Create
{
    internal sealed class CreateNotificationTypeCoverGapExtensionHandler : ICommandHandler<CreateNotificationTypeCoverGapExtensionCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;

        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateNotificationTypeCoverGapExtensionCommand, NotificationTypeCoverGapExtension>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));
        });
        
        public CreateNotificationTypeCoverGapExtensionHandler(DocumentManagementDbContext dbContext, IIdentityService identityService)
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