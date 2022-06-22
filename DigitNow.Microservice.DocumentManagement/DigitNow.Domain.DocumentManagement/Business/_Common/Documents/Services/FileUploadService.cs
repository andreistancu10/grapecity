using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services;

public class FileUploadService
{
    private readonly int _filesPerDirectory;
    private readonly string _completeRootPath;
    private readonly string _partition;

    public FileUploadService(
        string rootPath = "DigitNow_Documents",
        int filesPerDirectory = 1000,
        string partition = "C")
    {
        _filesPerDirectory = filesPerDirectory;
        _completeRootPath = $"{partition}:\\{rootPath}";
        _partition = partition;
    }

    public async Task UploadFileAsync(IFormFile formFileStream, string filename)
    {
        var fullPath = GenerateFullPath();
        fullPath = EnsureEligibilityOfPath(fullPath: fullPath);
        filename = EnsureEligibilityOfFilename(filename: filename, fullPath: fullPath);
        
        CheckOrCreateDirectoryTree(fullPath);

        await SaveFileOnDiskAsync(fullPath, filename, formFileStream);
    }

    private static string EnsureEligibilityOfFilename(string filename, string fullPath)
    {
        var isFilenameEligible = false;
        var newFilename = filename;

        for (var i = 1; !isFilenameEligible; i++)
        {
            if (DoesFileAlreadyExist(fullPath, newFilename))
            {
                var bits = filename.Split(".");
                newFilename = $"{bits[0]}_{i}.{bits[1]}";
            }
            else
            {
                isFilenameEligible = true;
            }
        }

        return newFilename;
    }

    private string EnsureEligibilityOfPath(string fullPath)
    {
        var isDirectoryEligible = false;
        var newFullPath = fullPath;


        for (var i = 1; !isDirectoryEligible; i++)
        {
            if (!IsRoomLeftInDirectory(newFullPath))
            {
                newFullPath = $"{fullPath}_{i}";
            }
            else
            {
                isDirectoryEligible = true;
            }
        }

        return newFullPath;
    }

    private static bool DoesFileAlreadyExist(string path, string filename)
    {
        if (!Directory.Exists(path))
        {
            return false;
        }

        var filenameWithPath = $"{path}\\{filename}";
        return Directory.EnumerateFiles(path).Any(c => c == filenameWithPath);
    }

    private bool IsRoomLeftInDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            return true;
        }

        return Directory.EnumerateFiles(path).Count() < _filesPerDirectory;
    }

    private static async Task SaveFileOnDiskAsync(string fullPath, string filename, IFormFile formFileStream)
    {
        if (formFileStream.Length <= 0)
        {
            return;
        }

        await using Stream fileStream = new FileStream($"{fullPath}\\{filename}", FileMode.Create, FileAccess.Write);
        await formFileStream.CopyToAsync(fileStream);
    }

    private string GenerateFullPath()
    {
        var year = DateTime.UtcNow.Year;
        var month = DateTime.UtcNow.Month;
        var day = DateTime.UtcNow.Day;

        return $"{_completeRootPath}\\{year}\\{month}\\{day}";
    }

    private static void CheckOrCreateDirectoryTree(string path)
    {
        if (!Directory.Exists(path) && !string.IsNullOrWhiteSpace(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}