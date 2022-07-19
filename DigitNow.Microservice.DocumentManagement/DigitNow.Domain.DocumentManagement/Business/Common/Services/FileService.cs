using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HTSS.Platform.Core.Files.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile formFileStream, string fileGuid);
        byte[] DownloadFileAsync(string relativePath, string fileGuid);
    }

    public class FileService : IFileService
    {
        private readonly int _filesPerDirectory;
        private readonly string _completeRootPath;
        private readonly string _partition;

        public FileService(
            string rootPath = "DigitNow_Documents",
            int filesPerDirectory = 1000,
            string partition = "C")
        {
            _filesPerDirectory = filesPerDirectory;
            _completeRootPath = $"{partition}:\\{rootPath}";
            _partition = partition;
        }

        public async Task<string> UploadFileAsync(IFormFile formFileStream, string fileGuid)
        {
            var relativePath = GenerateRelativePath();
            relativePath = EnsureEligibilityOfPath(relativePath);
            var fullPath = GenerateFullPath(relativePath);
            fileGuid = EnsureEligibilityOfFilename(fileGuid: fileGuid, fullPath: fullPath);

            CheckOrCreateDirectoryTree(fullPath);
            await SaveFileOnDiskAsync(fullPath, fileGuid, formFileStream);

            return relativePath;
        }

        public byte[] DownloadFileAsync(string relativePath, string fileGuid)
        {
            if (!DoesFileExist(GenerateFullPath(relativePath), fileGuid))
            {
                throw new FileNotFoundException();
            }

            return File.ReadAllBytes($"{_completeRootPath}\\{relativePath}\\{fileGuid}");
        }

        private static string EnsureEligibilityOfFilename(string fileGuid, string fullPath)
        {
            var isFilenameEligible = false;
            var newFilename = fileGuid;

            for (var i = 1; !isFilenameEligible; i++)
            {
                if (DoesFileExist(fullPath, newFilename))
                {
                    var bits = fileGuid.Split(".");
                    newFilename = $"{bits[0]}_{i}.{bits[1]}";
                }
                else
                {
                    isFilenameEligible = true;
                }
            }

            return newFilename;
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

        private static bool DoesFileExist(string path, string fileGuid)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            var fileGuidWithPath = $"{path}\\{fileGuid}";
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

        private static async Task SaveFileOnDiskAsync(string fullPath, string fileGuid, IFormFile formFileStream)
        {
            if (formFileStream.Length <= 0)
            {
                throw new EndOfStreamException();
            }

            await using Stream fileStream = new FileStream($"{fullPath}\\{fileGuid}", FileMode.Create, FileAccess.Write);
            await formFileStream.CopyToAsync(fileStream);
        }

        private string GenerateRelativePath()
        {
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var day = DateTime.UtcNow.Day;

            return $"{year}\\{month}\\{day}";
        }

        private string GenerateFullPath(string relativePath)
        {
            return $"{_completeRootPath}\\{relativePath}";
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