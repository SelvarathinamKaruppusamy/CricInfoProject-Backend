namespace CricInfo.Application.DTOs.AdminLogin;

public class ProfileDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MobileNo { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime Dob { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}