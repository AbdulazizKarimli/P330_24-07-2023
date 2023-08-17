namespace P330Pronia.Utils;

public static class Extensions
{
    public static bool CheckFileType(this IFormFile file, string fileType) 
        => file.ContentType.Contains(fileType);

    public static bool CheckFileSize(this IFormFile file, double size)
        => file.Length / 1024 < size;
}