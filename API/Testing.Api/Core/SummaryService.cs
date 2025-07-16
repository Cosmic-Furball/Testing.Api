using System.Text.Json;

namespace Testing.Api.Core;

public interface ISummaryService
{
    Task<List<string>> GetAllSummariesAsync();
    Task<bool> AddSummaryAsync(string summary);
    Task<bool> RemoveSummaryAsync(string summary);
    Task<bool> ReplaceSummariesAsync(List<string> summaries);
    Task<bool> SummaryExistsAsync(string summary);
}

public class SummaryService : ISummaryService
{
    private readonly string _summariesFilePath;
    private readonly ILogger<SummaryService> _logger;
    private readonly SemaphoreSlim _fileLock;

    public SummaryService(ILogger<SummaryService> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _summariesFilePath = Path.Combine(environment.ContentRootPath, "Summaries.json");
        _fileLock = new SemaphoreSlim(1, 1);
    }

    public async Task<List<string>> GetAllSummariesAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(_summariesFilePath))
            {
                _logger.LogWarning("Summaries.json file not found at {FilePath}", _summariesFilePath);
                return new List<string>();
            }

            var jsonContent = await File.ReadAllTextAsync(_summariesFilePath);
            var summaries = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();
            
            _logger.LogInformation("Retrieved {Count} summaries from file", summaries.Count);
            return summaries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading summaries from file");
            return new List<string>();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> AddSummaryAsync(string summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            _logger.LogWarning("Attempted to add null or empty summary");
            return false;
        }

        await _fileLock.WaitAsync();
        try
        {
            var summaries = await GetAllSummariesInternalAsync();
            
            // Check if summary already exists (case-insensitive)
            if (summaries.Any(s => string.Equals(s, summary, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning("Summary '{Summary}' already exists", summary);
                return false;
            }

            summaries.Add(summary);
            await SaveSummariesInternalAsync(summaries);
            
            _logger.LogInformation("Added new summary: '{Summary}'", summary);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding summary '{Summary}'", summary);
            return false;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> RemoveSummaryAsync(string summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            _logger.LogWarning("Attempted to remove null or empty summary");
            return false;
        }

        await _fileLock.WaitAsync();
        try
        {
            var summaries = await GetAllSummariesInternalAsync();
            
            // Find and remove summary (case-insensitive)
            var summaryToRemove = summaries.FirstOrDefault(s => 
                string.Equals(s, summary, StringComparison.OrdinalIgnoreCase));
            
            if (summaryToRemove == null)
            {
                _logger.LogWarning("Summary '{Summary}' not found for removal", summary);
                return false;
            }

            summaries.Remove(summaryToRemove);
            await SaveSummariesInternalAsync(summaries);
            
            _logger.LogInformation("Removed summary: '{Summary}'", summaryToRemove);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing summary '{Summary}'", summary);
            return false;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> ReplaceSummariesAsync(List<string> summaries)
    {
        if (summaries == null)
        {
            _logger.LogWarning("Attempted to replace summaries with null list");
            return false;
        }

        // Filter out null, empty, or whitespace-only summaries
        var validSummaries = summaries
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        await _fileLock.WaitAsync();
        try
        {
            await SaveSummariesInternalAsync(validSummaries);
            
            _logger.LogInformation("Replaced all summaries with {Count} new summaries", validSummaries.Count);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing summaries");
            return false;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> SummaryExistsAsync(string summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
            return false;

        var summaries = await GetAllSummariesAsync();
        return summaries.Any(s => string.Equals(s, summary, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<List<string>> GetAllSummariesInternalAsync()
    {
        if (!File.Exists(_summariesFilePath))
        {
            return new List<string>();
        }

        var jsonContent = await File.ReadAllTextAsync(_summariesFilePath);
        return JsonSerializer.Deserialize<List<string>>(jsonContent) ?? new List<string>();
    }

    private async Task SaveSummariesInternalAsync(List<string> summaries)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonContent = JsonSerializer.Serialize(summaries, options);
        await File.WriteAllTextAsync(_summariesFilePath, jsonContent);
    }
}
