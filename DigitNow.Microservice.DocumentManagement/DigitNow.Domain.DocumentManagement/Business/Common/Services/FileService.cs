using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IFileService
    {
        Task<Tuple<string, string>> UploadFileAsync(IFormFile formFileStream, string fileGuid);
        byte[] DownloadFileAsync(string path, string fileGuid);
    }

    public class FileService : IFileService
    {
        private readonly int _filesPerDirectory;
        private readonly string _rootPath;
        private readonly string _pathDirectorySeparator;

        public FileService(
            bool isLinuxTypePath = false,
            string rootDirectory = "DigitNow_Documents",
            int filesPerDirectory = 1000
            //string partition = "/home",
            )
        {
            _filesPerDirectory = filesPerDirectory;
            var partition = isLinuxTypePath ? "/home" : "C:";
            _pathDirectorySeparator = isLinuxTypePath ? "/" : "\\";
            _rootPath = $"{partition}{_pathDirectorySeparator}{rootDirectory}";
        }

        public async Task<Tuple<string, string>> UploadFileAsync(IFormFile formFileStream, string fileGuid)
        {
            var relativePath = GenerateRelativePath();
            relativePath = EnsureEligibilityOfPath(relativePath);
            var fullPath = GenerateFullPath(relativePath);
            fileGuid = EnsureEligibilityOfFilename(fileGuid, fullPath);

            CheckOrCreateDirectoryTree(fullPath);
            await SaveFileOnDiskAsync(fullPath, fileGuid, formFileStream);

            return new Tuple<string, string>(relativePath, fullPath);
        }

        public byte[] DownloadFileAsync(string path, string fileGuid)
        {
            if (!DoesFileExist(path, fileGuid))
            {
                throw new FileNotFoundException($"FilePath: {path}{_pathDirectorySeparator}{fileGuid}");
            }

            return File.ReadAllBytes($"{path}{_pathDirectorySeparator}{fileGuid}");
        }

        private string EnsureEligibilityOfFilename(string fileGuid, string fullPath)
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