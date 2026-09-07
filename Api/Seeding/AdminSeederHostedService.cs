using Application.Users.Commands.SeedAdmin;
using MediatR;
using Microsoft.Extensions.Options;

namespace Api.Seeding;

public sealed class AdminSeederHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<AdminSeedSettings> options,
    ILogger<AdminSeederHostedService> logger) : IHostedService
{
    private readonly AdminSeedSettings _settings = options.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_settings.Enabled)
        {
            logger.LogInformation(
                "Admin seeding is disabled; skipping.");
            return;
        }

        var command = new EnsureAdminUserCommand(
            _settings.FirstName,
            _settings.LastName,
            _settings.UserName,
            _settings.Email,
            _settings.Password,
            _settings.PhoneNumber);

        try
        {
            using var scope = scopeFactory.CreateScope();

            var sender = scope.ServiceProvider.GetRequiredService<ISender>();

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                logger.LogError(
                    "Could not seed the administrator: {ErrorCode} {ErrorMessage}",
                    result.Error.Code, result.Error.Message);
                return;
            }

            logger.LogInformation(
                "Administrator {AdminId} is in place for {Email}.",
                result.Value, _settings.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Seeding the administrator for {Email} failed.",
                _settings.Email);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
