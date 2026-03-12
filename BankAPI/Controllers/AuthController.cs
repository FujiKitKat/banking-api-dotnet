using BankAPI.DTO.Auth;
using BankAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = BankAPI.DTO.Auth.LoginRequest;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var token = await _authService.LogInAsync(loginRequest.Email, loginRequest.PasswordHash);

        if (token is null)
        {
            return  Unauthorized();
        }

        return Ok(token);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(CreateUserRequest createUserRequest)
    {
        var createUser = await _authService.CreateUserAsync(createUserRequest);
        
        return Ok(createUser);
    }
}