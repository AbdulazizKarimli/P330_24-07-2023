using P330Pronia.Exceptions;
using P330Pronia.Services.Interfaces;
using P330Pronia.Utils;
using F = System.IO;

namespace P330Pronia.Services.Implementations;

public class FileService : IFileService
{
    public async Task<string> CreateFileAsync(IFormFile file, string path, int maxFileSize, string fileType)
    {
        if (!file.CheckFileSize(maxFileSize))
            throw new FileSizeException("Olcusu o deyil");

        if (!file.CheckFileType(fileType))
            throw new FileTypeException("Shekil deyil");

        string fileName = $"{Guid.NewGuid()}-{file.FileName}";
        string resultPath = Path.Combine(path, fileName);
        using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return fileName;
    }

    public void DeleteFile(string path)
    {
        if (F.File.Exists(path))
        {
            F.File.Delete(path);
        }
    }
}