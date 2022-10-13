using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Commands.Update
{
    public class UpdatePublicAcquisitionHandler : ICommandHandler<UpdatePublicAcquisitionProjectCommand, ResultObject>
    {
        private readonly IPublicAcquisitionService _publicAcquisitionService;
        private readonly IMapper _mapper;

        public UpdatePublicAcquisitionHandler(IPublicAcquisitionService publicAcquisitionService, IMapper mapper)
        {
            _publicAcquisitionService = publicAcquisitionService;
            _mapper = mapper;
        }

        public async Task<ResultObject> Handle(UpdatePublicAcquisitionProjectCommand request, CancellationToken cancellationToken)
        {
            var publicAcquisitionFromDb = await _publicAcquisitionService.GetByIdQuery(request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (publicAcquisitionFromDb == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The public acquisition project with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.publicAquisitions.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            _mapper.Map(request, publicAcquisitionFromDb);

            await _publicAcquisitionService.UpdateAsync(publicAcquisitionFromDb, cancellationToken);

            return ResultObject.Ok();
        }
    }
}
