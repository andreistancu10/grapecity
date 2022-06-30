using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;

public class UploadFileHandler : ICommandHandler<UploadFileCommand, ResultObject>
{
    private readonly IMapper _mapper;
    private readonly IFileUploadService _fileUploadService;
    private readonly IUploadedFileService _uploadedFileService;

    public UploadFileHandler(IMapper mapper, IFileUploadService fileUploadService, IUploadedFileService uploadedFileService)
    {
        _mapper = mapper;
        _fileUploadService = fileUploadService;
        _uploadedFileService = uploadedFileService;
    }

    public async Task<ResultObject> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var newGuid = Guid.NewGuid();
        var filePath = await _fileUploadService.UploadFileAsync(request.File, newGuid.ToString());
        var newFile = await _uploadedFileService.CreateAsync(request, newGuid, filePath, cancellationToken);

        return ResultObject.Ok(_mapper.Map<UploadFileResponse>(newFile));
    }
}