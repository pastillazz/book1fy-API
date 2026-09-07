using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.Outbox;

public class OutboxSettings
{   
    public const string SectionName = "Outbox";
    [Required(ErrorMessage = "BatchSize is required.")]
    [Range(1,100, ErrorMessage = "BatchSize must be between 1 and 100.")]
    public int BatchSize { get; set; } 
    
    [Required(ErrorMessage = "MaxRetries is required.")]
    [Range(1,5, ErrorMessage = "MaxRetries must be between 1 and 10.")]
    public int MaxRetries { get; set; }
    
    [Required(ErrorMessage = "IntervalInSeconds is required.")]
    [Range(1, 3600, ErrorMessage = "IntervalInSeconds must be between 1 and 3600.")]
    public int IntervalInSeconds { get; set; } 
}