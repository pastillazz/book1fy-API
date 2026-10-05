
namespace Infrastructure.Google;

public sealed class GoogleAuthOptions
{
    public const string Section = "Google";
    public List<string> ClientIds { get; init; } = [];
}
