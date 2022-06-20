namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services;

public class FileUploadService
{
    private readonly int _depth;
    private readonly int _filesPerDirectory;
    private readonly string _filename;

    public FileUploadService(int depth, int filesPerDirectory, string filename)
    {
        _depth = depth;
        _filesPerDirectory = filesPerDirectory;
        _filename = filename;
    }
}