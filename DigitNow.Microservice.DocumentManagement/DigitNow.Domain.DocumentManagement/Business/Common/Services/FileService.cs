using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IFileService
    {
        Task<StoredFileModel> UploadFileAsync(FileModel fileModel, IFormFile contentFile);
        byte[] DownloadFileAsync(string path, string fileGuid);
    }

    public class FileService : IFileService
    {
        private readonly int _filesPerDirectory;
        private readonly string _rootPath;
        private readonly string _pathDirectorySeparator;
        private readonly IMapper _mapper;

        public FileService(
            IMapper mapper, 
            bool isLinuxTypePath = true,
            string rootDirectory = "DigitNow_Documents",
            int filesPerDirectory = 1000
            )
        {
            _mapper = mapper;
            _filesPerDirectory = filesPerDirectory;
            var partition = isLinuxTypePath ? "/home" : "C:";
            _pathDirectorySeparator = isLinuxTypePath ? "/" : "\\";
            _rootPath = $"{partition}{_pathDirectorySeparator}{rootDirectory}";
        }

        public async Task<StoredFileModel> UploadFileAsync(FileModel fileModel, IFormFile contentFile)
        {
            var relativePath = GenerateRelativePath();
            relativePath = EnsureEligibilityOfPath(relativePath);
            var fullPath = GenerateFullPath(relativePath);

            var generatedName = Guid.NewGuid();

            CheckOrCreateDirectoryTree(fullPath);
            await SaveFileOnDiskAsync(fullPath, generatedName.ToString(), contentFile);

            var storedFileModel = _mapper.Map<StoredFileModel>(fileModel);
            storedFileModel.RelativePath = relativePath;
            storedFileModel.AbsolutePath = fullPath;
            storedFileModel.GeneratedName = generatedName;

            return storedFileModel;
        }

        public byte[] DownloadFileAsync(string path, string fileGuid)
        {
            if (!DoesFileExist(path, fileGuid))
            {
                throw new FileNotFoundException($"FilePath: {path}{_pathDirectorySeparator}{fileGuid}");
            }

            return File.ReadAllBytes($"{path}{_pathDirectorySeparator}{fileGuid}");
        }
        
        private string EnsureEligibilityOfPath(string relativePath)
        {
            var isDirectoryEligible = false;
            var newRelativePath = relativePath;

            for (var i = 1; !isDirectoryEligible; i++)
            {
                if (!IsRoomLeftInDirectory(newRelativePath))
                {
                    newRelativePath = $"{relativePath}_{i}";
                }
                else
                {
                    isDirectoryEligible = true;
                }
            }

            return newRelativePath;
        }

        private bool DoesFileExist(string path, string fileGuid)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            var fileGuidWithPath = $"{path}{_pathDirectorySeparator}{fileGuid}";
            return Directory.EnumerateFiles(path).Any(c => c == fileGuidWithPath);
        }

        private bool IsRoomLeftInDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return true;
            }

            return Directory.EnumerateFiles(path).Count() < _filesPerDirectory;
        }

        private async Task SaveFileOnDiskAsync(string fullPath, string fileGuid, IFormFile formFileStream)
        {
            if (formFileStream.Length <= 0)
            {
                throw new EndOfStreamException();
            }

            await using Stream fileStream = new FileStream($"{fullPath}{_pathDirectorySeparator}{fileGuid}", FileMode.Create, FileAccess.Write);
            await formFileStream.CopyToAsync(fileStream);
        }

        private string GenerateRelativePath()
        {
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var day = DateTime.UtcNow.Day;

            return $"{year}{_pathDirectorySeparator}{month}{_pathDirectorySeparator}{day}";
        }

        private string GenerateFullPath(string relativePath)
        {
            return $"{_rootPath}{_pathDirectorySeparator}{relativePath}";
        }

        private static void CheckOrCreateDirectoryTree(string path)
        {
            if (!Directory.Exists(path) && !string.IsNullOrWhiteSpace(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}