using Api.Abstractions;
using Api.Authentication;
using Application.Users.Commands;
using Application.Users.Commands.Login;
using Application.Users.Commands.LoginWithRefreshToken;
using Application.Users.Commands.Register;
using Application.Users.Commands.RevokeRefreshTokens;
using Application.Users.Queries;
using Application.Users.Queries.GetUserByEmail;
using Application.Users.Queries.GetUserById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers;


[EnableRateLimiting("sliding")]
[ApiVersion(1)]
public class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.FirstName,
            request.LastName, request.UserName,
            request.Email, request.Password,
            request.PhoneNumber);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);

        SetAuthCookies(result.Value.Token, result.Value.RefreshToken); 
        return Ok(new AuthResponse(result.Value.Id, result.Value.Username, result.Value.Email));
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var command=new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);
        
        SetAuthCookies(result.Value.Token, result.Value.RefreshToken);
        return Ok(new AuthResponse(result.Value.Id, result.Value.Username, result.Value.Email));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EnableRateLimiting("token")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);

        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EnableRateLimiting("token")]
    public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var query = new GetUserByEmailQuery(email);
        var result = await Sender.Send(query, cancellationToken);
        if (result.IsFailure) return HandleFailure(result);
        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginUserWithRefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token is missing." });
        }
        var command = new LoginUserWithRefreshTokenCommand(refreshToken);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure) return HandleFailure(result);
        
        
        SetAuthCookies(result.Value.Token, result.Value.RefreshToken);
        Console.WriteLine($"[RECIBIDO] refresh={refreshToken?[..Math.Min(6, refreshToken.Length)]}");
        return Ok(new AuthResponse(result.Value.Id, result.Value.Username, result.Value.Email));
    }
    
    [HttpDelete("revoke-refresh-tokens")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RevokeRefreshTokens(CancellationToken cancellationToken)
    {
        var command = new RevokeRefreshTokensCommand();
        await Sender.Send(command, cancellationToken);
        
        ClearAuthCookies();
        return NoContent();
    }

    private void SetAuthCookies(string accessToken, string refreshToken)
    {
        Console.WriteLine($"[EMITIDO] refresh={refreshToken[..6]}");
        Response.Cookies.Append("accessToken", accessToken, new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(15)
        });
        
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/v1/users",
            Expires = DateTime.UtcNow.AddDays(7)
        });
        
    }
    private void ClearAuthCookies()
    {
        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            Path = "/api/v1/users"
        });
    }
}
