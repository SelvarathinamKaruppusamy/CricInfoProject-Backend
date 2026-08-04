namespace CricInfo.Application.DTOs.AdminLogin;

public class LoginResponseDto
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Token { get; set; }

    public string? UserName { get; set; }

    public string? Role { get; set; }

    public bool FirstLogin { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}