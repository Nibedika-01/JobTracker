using JobTracker.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace JobTracker.Presentation.Controllers;

public class AuthController: ControllerBase
{
    private readonly UserManager<IdentityUser> _users;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<IdentityUser> users, IConfiguration configuration)
    {
        _users = users;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
        var result = await _users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        var valid = await _users.CheckPasswordAsync(user, dto.Password);
        if (!valid) return Unauthorized(new { message = "Invalid credentials" });

        var token = GenerateJwt(user);
        return Ok(token);
    }

    private AuthResponseDto GenerateJwt(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)); //takes key from config then converts into bytes and sraps in security key obj
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //creates signing credentials using the key and specifying the hashing algorithm
        var expires = DateTime.UtcNow.AddDays(7);


        //claims are the data stored in the token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        //creates the JWT token using the specified parameters: issuer, audience, claims, expiration time, and signing credentials
        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        //converts the JWT token into a string format that can be sent to the client and returns response, string is sent to frontend
        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            Email = user.Email!,
            Expires = expires
        };
    }
}