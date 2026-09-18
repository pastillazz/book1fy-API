using System.ComponentModel.DataAnnotations;

namespace Infrastructure;

public class DatabaseOptions
{
    public const string SectionName = "DatabaseOptions";
    
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
    
    [Range(0, 10)]
    public int MaxRetryCount { get; set; }
    
    [Range(1, 300)]
    public int CommandTimeout { get; set; }

    public bool EnableDetailedErrors { get; set; }
    
    public bool EnableSensitiveDataLogging { get; set; }

}
