using System.Text;
using Application.Common.Abstractions.Authentication;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtTokenGenerator
    (IOptions<JwtSettings> options, TimeProvider time) :IJwtTokenGenerator
{   
    private readonly JwtSettings _jwtSettings = options.Value;

    public string Generate(User user)
    {
        var now = time.GetUtcNow().UtcDateTime;
        
        var signingCredentials=new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);
        
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMinutes(_jwtSettings.ExpiryMinutes),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Email]=user.Email.Value,
                [JwtRegisteredClaimNames.Jti]=Guid.NewGuid().ToString()
            },
            SigningCredentials = signingCredentials
        };
        
        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}