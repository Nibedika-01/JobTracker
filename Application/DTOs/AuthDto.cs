namespace JobTracker.Application.DTOs;

public class RegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime Expires { get; set; }
}