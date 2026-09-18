using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Infrastructure.Health;

public class DatabaseHealthCheck:IHealthCheck
{
   private readonly DatabaseOptions _databaseOptions;

   public DatabaseHealthCheck(IOptions<DatabaseOptions> databaseOptions)
    {
        _databaseOptions = databaseOptions.Value; 
    }
   
    
    public async Task<HealthCheckResult> 
        CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection=new Npgsql.NpgsqlConnection(_databaseOptions
                .ConnectionString);
            await connection.OpenAsync(cancellationToken);
            
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1;";
            await command.ExecuteScalarAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database is reachable.");
        }
        catch (Exception e)
        {
            return HealthCheckResult
                .Unhealthy("Database is not reachable.", e);
        }
    }
}
