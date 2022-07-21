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
        Task<UploadedFile> CreateAsync(UploadFileCommand request, Guid newGuid, string relativeFilePath, string absoluteFilePath, CancellationToken cancellationToken);
        Task CreateDocumentUploadedFilesAsync(IEnumerable<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task UpdateDocumentUploadedFilesAsync(List<long> uploadedFileIds, Document document, CancellationToken cancellationToken);
        Task<List<UploadedFile>> GetUploadedFilesAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
        Task<List<UploadedFile>> FetchUploadedFiles(long documentId, CancellationToken cancellationToken);

        Task<bool> AssociateUploadedFileToDocumentAsync(long uploadedFileId, long documentId,
            CancellationToken cancellationToken);
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
            string relativeFilePath, 
            string absoluteFilePath,
            CancellationToken cancellationToken)
        {
            var newFile = _mapper.Map<UploadedFile>(request);
            newFile.Guid = newGuid;
            newFile.RelativePath = relativeFilePath;
            newFile.AbsolutePath = absoluteFilePath;

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
            var documentUploadedFilesToRemove = document.DocumentUploadedFiles.Where(c => uploadedFileIdsToRemove.Contains(c.UploadedFileId));
            var idsToAdd = uploadedFileIds.Where(c => !currentlyPresentIds.Contains(c));
            _dbContext.DocumentUploadedFiles.RemoveRange(documentUploadedFilesToRemove);

            await CreateDocumentUploadedFilesAsync(idsToAdd, document, cancellationToken);
        }

        public async Task<List<UploadedFile>> GetUploadedFilesAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return await _dbContext.UploadedFiles.Where(c => ids.Contains(c.Id)).ToListAsync(cancellationToken);
        }

        public Task<List<UploadedFile>> FetchUploadedFiles(long documentId, CancellationToken cancellationToken)
        {
            return _dbContext.DocumentUploadedFiles
                .AsNoTracking()
                .Where(c => c.DocumentId == documentId)
                .Select(c => c.UploadedFile)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AssociateUploadedFileToDocumentAsync(long uploadedFileId, long documentId,
            CancellationToken cancellationToken)
        {
            var uploadedFileExist = await _dbContext.UploadedFiles.AnyAsync(c => c.Id == uploadedFileId, cancellationToken);
            var documentExist = await _dbContext.Documents.AnyAsync(c => c.Id == documentId, cancellationToken);


            if (!(uploadedFileExist && documentExist))
            {
                return false;
            }

            var newDocumentUploadedFile = new DocumentUploadedFile
            {
                UploadedFileId = uploadedFileId,
                DocumentId = documentId
            };

            await _dbContext.DocumentUploadedFiles.AddAsync(newDocumentUploadedFile, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}