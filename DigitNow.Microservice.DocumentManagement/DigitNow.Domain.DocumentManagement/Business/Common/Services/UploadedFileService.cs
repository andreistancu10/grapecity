using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles;
using DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFiles;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IUploadedFileService
    {
        Task<UploadedFile> CreateAsync(UploadFileCommand request, Guid newGuid, string filePath, CancellationToken cancellationToken);
        Task CreateDocumentUploadedFilesAsync(IEnumerable<long> uploadedFileIds, Document document,
            CancellationToken cancellationToken);
        Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document,
            CancellationToken cancellationToken);
        Task<List<UploadedFile>> GetUploadedFilesAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
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

        public async Task CreateDocumentUploadedFilesAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var uploadedFiles = await GetUploadedFilesAsync(uploadedFileIds, cancellationToken);
            var documentUploadedFiles = uploadedFiles.Select(c => new DocumentUploadedFile
            {
                UploadedFile = c,
                Document = document
            });

            await _dbContext.DocumentUploadedFiles.AddRangeAsync(documentUploadedFiles, cancellationToken);
        }

        public async Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken)
        {
            var currentlyPresentIds = document.DocumentUploadedFiles.Select(c => c.UploadedFileId).ToList();
            var uploadedFileIdsToRemove = currentlyPresentIds.Where(c => !uploadedFileIds.Contains(c));
            var documentUploadedFilesToRemove = document.DocumentUploadedFiles.Where(c=> uploadedFileIdsToRemove.Contains(c.UploadedFileId));
            var idsToAdd = uploadedFileIds.Where(c => !currentlyPresentIds.Contains(c));
            _dbContext.DocumentUploadedFiles.RemoveRange(documentUploadedFilesToRemove);

            await CreateDocumentUploadedFilesAsync(idsToAdd, document, cancellationToken);
        }

        public async Task<List<UploadedFile>> GetUploadedFilesAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return await _dbContext.UploadedFiles.Where(c => ids.Contains(c.Id)).ToListAsync(cancellationToken);
        }
    }
}