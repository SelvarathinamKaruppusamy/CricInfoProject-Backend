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

    // Keep this in sync with the expiry used in GenerateToken
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(5);

    public AuthService(
        CricDbContext context,
        IConfiguration configuration,
        EmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var user = await _context.Admins
            .FirstOrDefaultAsync(x => x.UserName == dto.UserName);

        if (user == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid username or password."
            };
        }

        if (user.IsLoggedIn)
        {
            // If the last issued token has already expired, the "logged in" flag
            // is stale (e.g. the browser was closed before it could call /logout).
            // Treat that as not logged in instead of permanently locking the account.
            bool sessionExpired = user.TokenExpiry.HasValue && user.TokenExpiry < DateTime.Now;

            if (!sessionExpired)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "You are already signed in on another device."
                };
            }
        }

        bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!valid)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid username or password."
            };
        }

        user.IsLoggedIn = true;
        user.TokenExpiry = DateTime.Now.Add(TokenLifetime);

        await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful.",

            Token = GenerateToken(user),
            UserName = user.UserName,
            Role = user.Role,
            FirstLogin = user.FirstLogin,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<bool> Logout(string username)
    {
        var user = await _context.Admins
            .FirstOrDefaultAsync(x => x.UserName == username);

        if (user == null)
            return false;

        user.IsLoggedIn = false;
        user.TokenExpiry = null;

        await _context.SaveChangesAsync();

        return true;
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
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.Add(TokenLifetime),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> ResetPassword(ResetPasswordDto dto)
    {
        var user = await _context.Admins
            .FirstOrDefaultAsync(x => x.UserName == dto.UserName);

        if (user == null)
        {
            return false;
        }

        bool valid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

        if (!valid)
        {
            return false;
        }

        if (!user.FirstLogin)
        {
            return false;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.FirstLogin = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ProfileDto?> GetProfile(string username)
    {
        var user = await _context.Admins
            .FirstOrDefaultAsync(x => x.UserName == username);

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

    public async Task<bool> UpdateProfile(int id, ProfileDto dto)
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

            try
            {
                await _emailService.SendMail(
                    dto.Email,
                    dto.UserName,
                    dto.Password);
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