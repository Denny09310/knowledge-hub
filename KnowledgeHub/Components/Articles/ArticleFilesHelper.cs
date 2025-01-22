namespace KnowledgeHub.Components.Articles;

public static class ArticleFilesHelper
{
    public static string GenerateFilePath(string folderPath, string fileName, string extension = ".md")
    {
        var fileNameWithExtension = $"{fileName}{extension}";
        return Path.Combine(folderPath, fileNameWithExtension);
    }

    public static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        return $"{Guid.NewGuid():N}{extension}";
    }

    public static string GetUploadFolderPath(IConfiguration configuration, string key)
    {
        var folderPath = configuration[$"Uploads:{key}"] ?? throw new InvalidOperationException($"{key} path not set.");
        Directory.CreateDirectory(folderPath);
        return folderPath;
    }
}