using Microsoft.AspNetCore.Mvc;
using Roomly.Booking.Services;
using Roomly.Booking.ViewModels;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Booking.Controllers;

[ApiController]
[Route("/api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingViewModel bookingViewModel)
    {
        try
        {
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

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetBookings(Guid userId)
    {
        try
        {
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{bookingId:guid}/cancel")]
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
}