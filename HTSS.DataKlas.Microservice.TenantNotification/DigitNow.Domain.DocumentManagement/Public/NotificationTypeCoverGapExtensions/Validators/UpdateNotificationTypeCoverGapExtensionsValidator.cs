using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Models;
using ShiftIn.Utils.Helpers;
using System;
using System.Linq;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Validators
{
    public class UpdateNotificationTypeCoverGapExtensionsValidator : AbstractValidator<UpdateNotificationTypeCoverGapExtensionsRequest>
    {
        public UpdateNotificationTypeCoverGapExtensionsValidator()
        {
            RuleFor(item => item.Active).NotNull();
            RuleFor(item => item.NotificationTypeId).NotNull();
            RuleFor(item => item.NotificationTypeStatus).NotNull();
            RuleFor(item => item.NotificationTypeActionId).NotNull();
            RuleFor(item => item.NotificationTypeActor).NotNull();
        }
    }
}