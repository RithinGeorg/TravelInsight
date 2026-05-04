using Microsoft.AspNetCore.Mvc;
using TravelInsight.Application.DTOs;
using TravelInsight.Application.Services;

namespace TravelInsight.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _auth.LoginAsync(request, ct);
        if (result is null) return Unauthorized(new { error = "Invalid email or password." });

        Response.Cookies.Append("travelinsight_user", result.User.Email, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(2)
        });

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(RegisterRequest request, CancellationToken ct) =>
        Ok(await _auth.RegisterAsync(request, ct));

    [HttpPost("oauth-demo")]
    public async Task<ActionResult<LoginResponse>> OAuthDemo(OAuthDemoRequest request, CancellationToken ct) =>
        Ok(await _auth.OAuthDemoAsync(request, ct));
}
