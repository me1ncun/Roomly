using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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

    public AuthController(
        IUserService userService,
        IHttpContextAccessor contextAccessor)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(loginViewModel.Email);

            _contextAccessor.HttpContext.Response.Cookies.Append("token", user.Token, new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(user);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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
            await _userService.CreateUserAsync(registrationViewModel);
            
            var user = await _userService.GetUserByEmailAsync(registrationViewModel.Email);
                    
            _contextAccessor.HttpContext.Response.Cookies.Append("token", user.Token,  new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None 
            });

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
}