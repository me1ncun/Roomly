using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roomly.Booking.Services;
using Roomly.Booking.ViewModels;
using Roomly.Users.Infrastructure.Exceptions;
using StackExchange.Redis;

namespace Roomly.Booking.Controllers;

[ApiController]
[Route("/api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookingController(
        IBookingService bookingService,
        IHttpContextAccessor httpContextAccessor)
    {
        _bookingService = bookingService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingCreateViewModel bookingViewModel)
    {
        try
        {
            await _bookingService.CreateBookingAsync(bookingViewModel, GetUserId());
            
            return Ok();
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        try
        {
            var userId = GetUserId();
            
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
                
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{bookingId:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid bookingId)
    {
        try
        {
           await _bookingService.CancelBookingAsync(bookingId);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    private Guid GetUserId()
    {
        var userIdClaim  = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        return Guid.Parse(userIdClaim);
    }
}