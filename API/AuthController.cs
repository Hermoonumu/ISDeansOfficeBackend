

namespace DeanInfoSystem.API;

using DeanInfoSystem.Application.Common.Auth;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService _authSvc,
                            IConfiguration _conf) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginFormDTO lfDTO)
    {
        Dictionary<string, string> tokens;
        tokens = await _authSvc.LoginAsync(lfDTO);
        await ComposeCookies(HttpContext, tokens);
        User user = await _authSvc.AuthenticateUserAsync(tokens["AccessToken"]);
        return Ok(UserMapper.UserToDTO(user));
    }

    [Authorize]
    [HttpGet("whoami")]
    public async Task<IActionResult> WhoAmI()
    {

        User? user = await _authSvc.AuthenticateUserAsync(HttpContext.Request.Cookies["AccessToken"]!);

        if (user is null) return Unauthorized();

        return Ok(UserMapper.UserToDTO(user));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (HttpContext.Request.Cookies["RefreshToken"] == null)
            return Unauthorized(new { Message = "Please log in first." });
        Dictionary<string, string> tokens = new();
        tokens = await _authSvc
                            .AttemptRefreshAsync(
                                HttpContext.Request.Cookies["RefreshToken"]!
                                );
        await ComposeCookies(HttpContext, tokens);
        return Ok();
    }


    private async Task ComposeCookies(HttpContext ctx, Dictionary<string, string> tokens, bool expired = false)
    {
        var CookieOpt = new CookieOptions()
        {
            HttpOnly = true,
            Secure = false,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        };
        if (expired) CookieOpt.Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(-1));
        if (!expired) CookieOpt.Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(Int32.Parse(_conf["Security:RefreshTokenExpirySpanDays"]!)));
        ctx.Response.Cookies.Append("RefreshToken", expired ? "" : tokens["RefreshToken"], CookieOpt);
        if (!expired) CookieOpt.Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(Int32.Parse(_conf["Security:AccessTokenExpirySpanMinutes"]!)));
        ctx.Response.Cookies.Append("AccessToken", expired ? "" : tokens["AccessToken"], CookieOpt);
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authSvc.ClearTokensAsync(HttpContext.Request.Cookies["AccessToken"]!,
                                        HttpContext.Request.Cookies["RefreshToken"]!);
        await ComposeCookies(HttpContext, [], true);
        return Ok();
    }
}