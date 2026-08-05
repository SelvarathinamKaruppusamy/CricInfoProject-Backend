public class Admin
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string PasswordHash { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public string MobileNo { get; set; }

    public string Role { get; set; }

    public string Address { get; set; }

    public DateTime Dob { get; set; }

    public bool FirstLogin { get; set; }
    public bool IsLoggedIn { get; set; } = false;
    public DateTime? TokenExpiry { get; set; }
}