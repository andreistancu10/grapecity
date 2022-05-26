using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.BusinessValidators.Abstractions;
using HTSS.Platform.Core.BusinessValidators.Abstractions.Attributes;
using HTSS.Platform.Core.BusinessValidators.Abstractions.Models;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Update.Validators
{
    [StaticValidationRule]
    public class UniqueCodeValidator : AbstractValidationRule<UpdateNotificationStatusCommand>
    {
        private readonly TenantNotificationDbContext _dbContext;

        public UniqueCodeValidator(TenantNotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<BusinessValidationResult> Validate(UpdateNotificationStatusCommand model,
            CancellationToken cancellationToken)
        {
            BusinessValidationResult validationResult = new BusinessValidationResult(true);

            List<NotificationStatus> notificationStatusesInDb = await _dbContext.NotificationStatuses
                .Where(x => x.Id != model.Id && x.Code == model.Code).ToListAsync(cancellationToken);

            if (notificationStatusesInDb.Any(x =>
                    string.Equals(x.Code, model.Code, StringComparison.OrdinalIgnoreCase)))
                validationResult.AddError(new ErrorMessage
                {
                    TranslationCode =
                        "tenant-notification.notification-status.backend.update.validation.codeIsNotUnique",
                    Message = $"A notificationStatus named {model.Name} for code {model.Code} already exists."
                });

            return validationResult;
        }
    }
}