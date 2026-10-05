using Application.Common.Google;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace Infrastructure.Google;

public class GoogleTokenValidator(IOptions<GoogleAuthOptions> options):IGoogleTokenValidator
{
    public async Task<GoogleUserInfo?> ValidateAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = options.Value.ClientIds
            };
            
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            
            return new GoogleUserInfo(
                payload.Subject,
                payload.Email,
                payload.FamilyName,
                payload.Name
            );
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}