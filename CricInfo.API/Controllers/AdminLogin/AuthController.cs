using CricInfo.Application.DTOs.AdminLogin;
using CricInfo.Application.Interfaces.Services.AdminLogin;
using CricInfo.Application.Services.AdminLoginModule;
using CricInfo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _service.Login(dto);

        if (result == null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult>
        ResetPassword(ResetPasswordDto dto)
    {
        var result =
            await _service.ResetPassword(dto);

        if (!result)
        {
            return BadRequest(
                new
                {
                    message =
                        "Invalid Username or Password"
                });
        }

        return Ok(
            new
            {
                message =
                    "Password Updated Successfully"
            });
    }
    [HttpGet("profile/{username}")]
    public async Task<IActionResult>
         GetProfile(
         string username)
    {
        var result =
            await _service
                .GetProfile(username);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    [HttpPut("profile/{id}")]
    public async Task<IActionResult> UpdateProfile(
        int id,
        ProfileDto dto)
    {
        var result = await _service.UpdateProfile(id, dto);

        if (!result)
        {
            return BadRequest(new
            {
                message = "Profile Update Failed"
            });
        }

        return Ok(new
        {
            message = "Profile Updated Successfully"
        });
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _service.Register(dto);

        if (!result)
        {
            return BadRequest(
                new
                {
                    Message = "Registration Failed"
                });
        }

        return Ok(
            new
            {
                Message = "Admin Registered Successfully"
            });
    }
}