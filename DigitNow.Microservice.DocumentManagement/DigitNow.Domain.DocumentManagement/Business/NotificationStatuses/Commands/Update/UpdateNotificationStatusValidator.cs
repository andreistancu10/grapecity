using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.BusinessValidators.Abstractions;
using HTSS.Platform.Core.BusinessValidators.Abstractions.Models;
using HTSS.Platform.Core.CQRS;
using MediatR;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Commands.Update
{
    public class UpdateNotificationStatusValidator : IPipelineBehavior<UpdateNotificationStatusCommand, ResultObject>
    {
        private readonly IBusinessValidator _businessValidator;

        public UpdateNotificationStatusValidator(IBusinessValidator businessValidator)
        {
            _businessValidator = businessValidator;
        }

        public async Task<ResultObject> Handle(UpdateNotificationStatusCommand request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<ResultObject> next)
        {
            BusinessValidationResult validationResult = await _businessValidator.Validate(request, cancellationToken);

            return validationResult.IsValid ? await next() : ResultObject.Error(validationResult.Errors);
        }
    }
}