using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Exceptions;
using Roomly.Users.Services;
using Roomly.Users.ViewModels;

namespace Roomly.Users.Controllers;

[ApiController]
[Route("api/users")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<User> _userManager;

    public AuthController(
        IUserService userService,
        IHttpContextAccessor contextAccessor,
        UserManager<User> userManager)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        try
        {
            var token = await _userService.GetUserTokenByCredentialsAsync(loginViewModel);

            _contextAccessor.HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(token);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (LoginException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel registrationViewModel)
    {
        try
        {
            var user = await _userService.CreateUserAsync(registrationViewModel);
            
            await _userManager.AddToRoleAsync(user, "User");

            return Ok();
        }
        catch (EntityAlreadyExistsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
    
    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        try
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete("token", new CookieOptions
            {
                MaxAge = TimeSpan.FromSeconds(1),
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None
            });
        
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}