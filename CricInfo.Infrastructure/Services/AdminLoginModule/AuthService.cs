using BCrypt.Net;

using CricInfo.Application.DTOs.AdminLogin;
using CricInfo.Application.Interfaces.Services.AdminLogin;

using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CricInfo.Application.Services.AdminLoginModule;

public class AuthService : IAuthService
{
    private readonly CricDbContext _context;

    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public AuthService(
        CricDbContext context,
        IConfiguration configuration,
        EmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<LoginResponseDto?> Login(LoginDto dto)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        var user = await _context.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.UserName == dto.UserName);

        Console.WriteLine(
            $"DB : {watch.ElapsedMilliseconds}");

        if (user == null)
            return null;

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        Console.WriteLine(
            $"BCrypt : {watch.ElapsedMilliseconds}");

        if (!valid)
            return null;

        var result = new LoginResponseDto
        {
            Token = GenerateToken(user),
            UserName = user.UserName,
            Role = user.Role,
            FirstLogin = user.FirstLogin,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        Console.WriteLine(
            $"TOTAL : {watch.ElapsedMilliseconds}");

        return result;
    }

    private string GenerateToken(Admin user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.Role)
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<bool>
ResetPassword(
    ResetPasswordDto dto)
    {
        var user =
            await _context.Admins
            .FirstOrDefaultAsync(
                x => x.UserName ==
                dto.UserName);

        if (user == null)
        {
            return false;
        }

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.CurrentPassword,
                user.PasswordHash);

        if (!valid)
        {
            return false;
        }

        if (!user.FirstLogin)
        {
            return false;
        }

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                dto.NewPassword);

        user.FirstLogin = false;

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<ProfileDto?>
    GetProfile(
    string username)
    {
        var user =
            await _context.Admins
            .FirstOrDefaultAsync(
                x => x.UserName == username);

        if (user == null)
        {
            return null;
        }

        return new ProfileDto
        {
            Id = user.Id,

            FirstName = user.FirstName,

            LastName = user.LastName,

            Email = user.Email,

            MobileNo = user.MobileNo,

            Gender = user.Gender,

            Dob = user.Dob,

            Address = user.Address,

            Role = user.Role
        };
    }
    public async Task<bool> UpdateProfile(
      int id,
      ProfileDto dto)
    {
        var user = await _context.Admins
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return false;
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.MobileNo = dto.MobileNo;
        user.Gender = dto.Gender;
        user.Dob = dto.Dob;
        user.Address = dto.Address;

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> Register(RegisterDto dto)
    {
        try
        {
            var exists = await _context.Admins
                .AnyAsync(x => x.UserName == dto.UserName);

            if (exists)
            {
                return false;
            }

            var admin = new Admin
            {
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Gender = dto.Gender,
                MobileNo = dto.MobileNo,
                Role = dto.Role,
                Address = dto.Address,
                Dob = dto.Dob,
                FirstLogin = true
            };

            _context.Admins.Add(admin);

            await _context.SaveChangesAsync();

            // Send Mail
            try
            {
                await _emailService.SendMail(
                    dto.Email,
                    dto.UserName,
                    dto.Password);

                Console.WriteLine("MAIL SENT");
            }
            catch (Exception ex)
            {
                Console.WriteLine("MAIL FAILED");
                Console.WriteLine(ex.Message);

                // Don't fail registration if mail fails
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.Message);
            }

            return false;
        }
    }
}