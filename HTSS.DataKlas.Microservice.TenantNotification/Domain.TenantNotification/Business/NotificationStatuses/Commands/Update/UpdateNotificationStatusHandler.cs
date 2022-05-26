﻿using System;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Update
{
    internal sealed class UpdateNotificationStatusHandler : ICommandHandler<UpdateNotificationStatusCommand, ResultObject>
    {
        private readonly TenantNotificationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        
        public UpdateNotificationStatusHandler(TenantNotificationDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.NotificationStatuses.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationStatus with id {request.Id} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification-status.backend.update.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });

            entity.Name = request.Name;
            entity.Code = request.Code;
            entity.Active = request.Active;
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedBy = _identityService.AuthenticatedUser.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}