using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.Health;

public class DatabaseHealthCheck(IConfiguration configuration):IHealthCheck
{
    private readonly string _connectionString =
        configuration
            .GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException();
    
    public async Task<HealthCheckResult> 
        CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection=new Npgsql.NpgsqlConnection(_connectionString);
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