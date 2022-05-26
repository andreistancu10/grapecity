using System;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Commands.Update
{
    internal sealed class UpdateNotificationTypeHandler : ICommandHandler<UpdateNotificationTypeCommand, ResultObject>
    {
        private readonly TenantNotificationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        
        public UpdateNotificationTypeHandler(TenantNotificationDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(UpdateNotificationTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.NotificationTypes.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationType with id {request.Id} does not exist.",
                    TranslationCode = "tenant-notification.notification-type.backend.update.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });

            entity.Name = request.Name;
            entity.Code = request.Code;
            entity.IsInformative = request.IsInformative;
            entity.IsUrgent = request.IsUrgent;
            entity.Active = request.Active;
            entity.EntityType = request.EntityType;
            entity.TranslationLabel = request.TranslationLabel;
            entity.Expression = request.Expression;
            entity.NotificationStatusId = request.NotificationStatusId;
            entity.Expression = request.Expression;
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedBy = _identityService.AuthenticatedUser.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}