﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Files.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries;

public class DownloadFileHandler : IQueryHandler<DownloadFileQuery, DownloadFileResponse>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public DownloadFileHandler(
        DocumentManagementDbContext dbContext,
        IMapper mapper, IFileService fileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<DownloadFileResponse> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var uploadedFile = await _dbContext.UploadedFiles.FirstAsync(c=>c.Id==request.FileId, cancellationToken);
        var fileBytes = _fileService.DownloadFileAsync(uploadedFile.RelativePath, uploadedFile.Guid.ToString());

        return 
            new DownloadFileResponse(new FileContent(uploadedFile.Name, uploadedFile.ContentType, fileBytes));
    }
}