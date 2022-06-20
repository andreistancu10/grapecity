using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services;

public class FileUploadService
{
    private readonly int _depth;
    private readonly int _filesPerDirectory;
    private readonly string _filename;
    private readonly int _chunkSize;
    private readonly string _completeRootPath;
    private readonly string _partition;

    public FileUploadService(int depth,
        int filesPerDirectory,
        string filename,
        string rootPath,
        int chunkSize,
        string partition = "C")
    {
        _depth = depth;
        _filesPerDirectory = filesPerDirectory;
        _filename = filename;
        _chunkSize = chunkSize;
        _completeRootPath = $"{partition}:\\{rootPath}";
        _partition = partition;
    }

    public async Task UploadFileAsync(IFormFile formFileStream, string filename)
    {
        var hash = HashString(filename);
        filename = $"{hash}_{filename}";

        var chunks = SplitIntoChunks(hash);
        var fullPath = GenerateFullPath(chunks);

        CheckOrCreateDirectoryTree(fullPath);

        //TODO: make sure there aren't 1000 files in that path already
        //TODO: check if a file with same name already exists

        await SaveFileOnDiskAsync(fullPath, filename, formFileStream);
    }

    private static string HashString(string filename)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(filename);
        var hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }

    private static async Task SaveFileOnDiskAsync(string fullPath, string filename, IFormFile formFileStream)
    {
        if (formFileStream.Length <=0)
        {
            return;
        }

        await using Stream fileStream = new FileStream($"{fullPath}\\{filename}", FileMode.Create, FileAccess.Write);
        await formFileStream.CopyToAsync(fileStream);
    }

    private string GenerateFullPath(IEnumerable<string> chunks)
    {
        var stringBuilder = new StringBuilder(_completeRootPath).AppendJoin("\\", chunks);
        return stringBuilder.ToString();
    }

    private static void CheckOrCreateDirectoryTree(string path)
    {
        if (!Directory.Exists(path) && !string.IsNullOrWhiteSpace(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private IEnumerable<string> SplitIntoChunks(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return Array.Empty<string>();
        }

        if (str.Length < _depth * _chunkSize)
        {
            throw new Exception("String is shorter than expected.");
        }

        var chunks = new List<string>();

        for (var i = 0; i < _depth; i++)
        {
            chunks.Add(str.Substring(i * _chunkSize, _chunkSize));
        }

        return chunks;
    }
}