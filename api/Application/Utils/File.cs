namespace Application.Utils;

public static class FileUtilities
{
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>
    {
        // Imagens
        ".jpg", ".jpeg", ".png", ".gif", ".svg", ".bmp", ".webp", ".tiff", ".ico",
        
        // Documentos
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".odt", ".ods", ".odp", ".rtf", ".md",
        
        // Áudio
        ".mp3", ".wav", ".ogg", ".flac", ".aac", ".m4a",
        
        // Vídeo
        ".mp4", ".mov", ".avi", ".mkv", ".flv", ".wmv", ".webm",
        
        // Arquivos compactados
        ".zip", ".gz", ".rar", ".tar", ".7z", ".bz2", ".xz",
        
        // Outros
        ".csv", ".xml", ".json"
    };

    private static readonly List<string> AllowedMimeTypes = new List<string>
    {
        // Imagens
        "image/jpeg", "image/png", "image/gif", "image/svg+xml", "image/bmp", "image/webp", "image/tiff", "image/x-icon",
        
        // Documentos
        "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/plain",
        "application/vnd.oasis.opendocument.text", "application/vnd.oasis.opendocument.spreadsheet", "application/vnd.oasis.opendocument.presentation",
        "application/rtf", "text/markdown",
        
        // Áudio
        "audio/mpeg", "audio/wav", "audio/ogg", "audio/flac", "audio/aac", "audio/mp4",
        
        // Vídeo
        "video/mp4", "video/quicktime", "video/x-msvideo", "video/x-matroska", "video/x-flv", "video/x-ms-wmv", "video/webm",
        
        // Arquivos compactados
        "application/zip", "application/gzip", "application/x-rar-compressed", "application/x-tar", "application/x-7z-compressed", "application/x-bzip2", "application/x-xz",
        
        // Outros
        "text/csv", "application/xml", "application/json"
    };

    public static bool IsAllowedExtension(string extension) => AllowedExtensions.Contains(extension);

    public static bool IsAllowedMimeType(string mimeType) => AllowedMimeTypes.Contains(mimeType);
    public static bool IsAllowedFile(string extension, string mimeType) => IsAllowedExtension(extension) && IsAllowedMimeType(mimeType);

    public static string GetExtension(string path) => Path.GetExtension(path).ToLowerInvariant();
}
