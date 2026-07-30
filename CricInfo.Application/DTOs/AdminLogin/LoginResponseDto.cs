namespace CricInfo.Application.DTOs.AdminLogin;

public class LoginResponseDto
{
    public string Token { get; set; }

    public string UserName { get; set; }

    public string Role { get; set; }

    public bool FirstLogin { get; set; }
}