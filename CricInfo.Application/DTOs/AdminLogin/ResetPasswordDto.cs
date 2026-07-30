namespace CricInfo.Application.DTOs.AdminLogin;

public class ResetPasswordDto
{
    public string UserName { get; set; }

    public string CurrentPassword { get; set; }

    public string NewPassword { get; set; }
}