using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webapplication.Models;
using webapplication.Services;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly UserContext userContext;
    readonly ITokenService tokenService;

    public AuthController(UserContext userContext, ITokenService tokenService)
    {
        this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    // GET api/values
    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] Login login)
    {
        if (login == null)
        {
            return BadRequest("Invalid client request");
        }

        var user = userContext.Logins.
            FirstOrDefault(l => l.UserName == login.UserName && l.Password == login.Password);

        if (user == null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, login.UserName),
            new Claim(ClaimTypes.Role, "Manager")
        };

        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        userContext.SaveChanges();

        return Ok(new
        {
            Token = accessToken,
            refreshToken = refreshToken
        });
    }
}