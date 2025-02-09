using MassTransit;
using Microsoft.EntityFrameworkCore;
using Roomly.Booking.ViewModels;
using Roomly.Rooms.ViewModels;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Shared.Data.Enums;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Booking.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BookingService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public BookingService(
        ApplicationDbContext dbContext,
        ILogger<BookingService> logger,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task CreateBookingAsync(BookingViewModel bookingViewModel)
    {
        
    }

    public async Task<List<BookingViewModel>> GetUserBookingsAsync(Guid userId)
    {
        var bookings = await _dbContext.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .Where(b => b.UserId == userId)
            .Select(b => new BookingViewModel()
            {
                UserName = b.User.Name,
                RoomName = b.Room.Name,
                UserEmail = b.User.Email,
                RoomLocation = b.Room.Location,
                RoomCapacity = b.Room.Capacity,
                RoomType = b.Room.Type,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
            })
            .ToListAsync();
        
        _logger.LogInformation($"User bookings retrieved {bookings.Count} bookings.");

        return bookings;
    }

    public async Task CancelBookingAsync(Guid bookingId)
    {
       var booking = await _dbContext.Bookings.FindAsync(bookingId);
        if (booking is null)
            throw new EntityNotFoundException();

        booking.Status = BookingStatus.Cancelled;
        
        _logger.LogInformation($"Booking with id {bookingId} has been cancelled.");
        
        await _dbContext.SaveChangesAsync();
    }
}

public interface IBookingService
{
    Task CreateBookingAsync(BookingViewModel bookingViewModel);
    Task<List<BookingViewModel>> GetUserBookingsAsync(Guid userId);
    Task CancelBookingAsync(Guid bookingId);
}