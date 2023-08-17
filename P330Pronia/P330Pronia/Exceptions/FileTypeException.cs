namespace P330Pronia.Exceptions;

public sealed class FileTypeException : Exception
{
    public FileTypeException(string message) : base(message)
    {
    }
}