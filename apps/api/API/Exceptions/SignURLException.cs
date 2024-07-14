namespace API.Exceptions;

public class SignURLException(string fileKey, string message) : Exception(message)
{
    public string FileKey { get; set; } = fileKey;
}