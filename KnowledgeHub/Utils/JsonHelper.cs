using System.Text.Json;

namespace KnowledgeHub.Utils;

public class JsonHelper(IWebHostEnvironment env)
{
    private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    private readonly IWebHostEnvironment _env = env;

    /// <summary>
    /// Reads a JSON file from the wwwroot directory and deserializes it into a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize into.</typeparam>
    /// <param name="fileName">The name of the JSON file (including path relative to wwwroot).</param>
    /// <returns>A list of objects of type T.</returns>
    public async Task<List<T>> ReadJsonAsync<T>(string fileName)
    {
        try
        {
            // Combine the path to the wwwroot folder and the file name.
            var path = Path.Combine(_env.WebRootPath, fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"The file '{fileName}' does not exist in wwwroot.");

            var content = await File.ReadAllTextAsync(path);
            var data = JsonSerializer.Deserialize<List<T>>(content, _options);

            return data ?? [];
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            throw new InvalidOperationException($"Error reading JSON file '{fileName}': {ex.Message}");
        }
    }
}

