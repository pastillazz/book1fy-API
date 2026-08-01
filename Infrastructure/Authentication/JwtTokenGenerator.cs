using System.Text;
using Application.Abstractions.Authentication;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtTokenGenerator(IOptions<JwtSettings> options):IJwtGenerator
{   
    private readonly JwtSettings _jwtSettings = options.Value;
    public string Generate(User user)
    {
        var signingCredentials=new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);
        
        throw new NotImplementedException();
    }
}