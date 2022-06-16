using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using HTSS.Platform.Core.BusinessValidators.Abstractions;
using HTSS.Platform.Core.BusinessValidators.Abstractions.Attributes;
using HTSS.Platform.Core.BusinessValidators.Abstractions.Models;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Commands.Update.Validators
{
    [StaticValidationRule]
    public class UniqueCodeValidator : AbstractValidationRule<UpdateNotificationStatusCommand>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public UniqueCodeValidator(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<BusinessValidationResult> Validate(UpdateNotificationStatusCommand model,
            CancellationToken cancellationToken)
        {
            var validationResult = new BusinessValidationResult(true);

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