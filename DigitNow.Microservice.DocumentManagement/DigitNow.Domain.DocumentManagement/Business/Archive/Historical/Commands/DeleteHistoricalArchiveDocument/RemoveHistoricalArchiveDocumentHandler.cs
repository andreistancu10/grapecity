using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Historical.Commands.RemoveHistoricalArchiveDocument
{
    public class RemoveHistoricalArchiveDocumentHandler : ICommandHandler<RemoveHistoricalArchiveDocumentCommand, ResultObject>
    {
        private readonly IDynamicFormsService _dynamicFormsService;

        public RemoveHistoricalArchiveDocumentHandler(IDynamicFormsService dynamicFormsService)
        {
            _dynamicFormsService = dynamicFormsService;
        }
        public async Task<ResultObject> Handle(RemoveHistoricalArchiveDocumentCommand request, CancellationToken cancellationToken)
        {
            var hasFormFillingLog = await _dynamicFormsService.DynamicFormFillingLogExists(request.Id, cancellationToken);
            if (!hasFormFillingLog)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The historical archive document with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.archive.historical.backend.deleteDocument.IdNotFound",
                    Parameters = new object[] { request.Id }
                });

            await _dynamicFormsService.RemoveDynamicFormFillingLogAsync(request.Id, cancellationToken);
            
            return ResultObject.Ok();
        }
    }
}
