using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services;

public interface IUploadedFileService
{
    Task<UploadedFile> CreateAsync(UploadFileCommand request, Guid newGuid, string filePath, CancellationToken cancellationToken);
}

public class UploadedFileService : IUploadedFileService
{
    private readonly IMapper _mapper;
    private readonly DocumentManagementDbContext _dbContext;

    public UploadedFileService(IMapper mapper, DocumentManagementDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<UploadedFile> CreateAsync(
        UploadFileCommand request,
        Guid newGuid,
        string filePath,
        CancellationToken cancellationToken)
    {
        var newFile = _mapper.Map<UploadedFile>(request);
        newFile.Guid = newGuid;
        newFile.RelativePath = filePath;

        await _dbContext.UploadedFiles.AddAsync(newFile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return newFile;
    }
}