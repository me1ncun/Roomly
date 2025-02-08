using Microsoft.EntityFrameworkCore;
using Roomly.Booking.ViewModels;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Booking.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BookingService> _logger;

    public BookingService(
        ApplicationDbContext dbContext,
        ILogger<BookingService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task CreateBookingAsync(BookingViewModel bookingViewModel)
    {
        
    }

    public async Task GetUserBookingsAsync(string userId)
    {
        
    }

    public async Task CancelBookingAsync(string bookingId)
    {
        
    }
    
}

public interface IBookingService
{
    
}