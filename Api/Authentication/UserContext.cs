using Application.Abstractions.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Authentication;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        TryGetUserId(out var userId)
            ? userId
            : throw new InvalidOperationException("User is not authenticated.");
    
    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;

        var subject = httpContextAccessor.HttpContext?.User
            .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return subject is not null && Guid.TryParse(subject, out userId);
    }
}
