using MassTransit;
using Microsoft.EntityFrameworkCore;
using Roomly.Booking.ViewModels;
using Roomly.Rooms.ViewModels;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Enums;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Booking.Services;

public interface IBookingService
{
    Task CreateBookingAsync(BookingCreateViewModel bookingViewModel);
    Task<List<BookingViewModel>> GetUserBookingsAsync(Guid userId);
    Task CancelBookingAsync(Guid bookingId);
}

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BookingService> _logger;
    private readonly IRequestClient<AvailabilityRoomViewModel> _requestClient;

    public BookingService(
        ApplicationDbContext dbContext,
        ILogger<BookingService> logger,
        IRequestClient<AvailabilityRoomViewModel> requestClient)
    {
        _dbContext = dbContext;
        _logger = logger;
        _requestClient = requestClient;
    }

    public async Task CreateBookingAsync(BookingCreateViewModel bookingViewModel)
    {
        var availabilityViewModel = new AvailabilityRoomViewModel()
        {
            UserId = bookingViewModel.UserId,
            RoomId = bookingViewModel.RoomId,
            StartTime = bookingViewModel.StartTime,
            EndTime = bookingViewModel.EndTime
        };
        
        _logger.LogInformation($"Checking availability for Room {bookingViewModel.RoomId} from {bookingViewModel.StartTime} to {bookingViewModel.EndTime}");
        
        var response = await _requestClient.GetResponse<AvailabilityResponse>(availabilityViewModel);

        if (!response.Message.IsAvailable)
        {
            _logger.LogWarning($"Room {bookingViewModel.RoomId} is not available for booking.");
            
            throw new Exception("Slot is not available");
        }

        var booking = new Shared.Data.Entities.Booking
        {
            UserId = bookingViewModel.UserId,
            RoomId = bookingViewModel.RoomId,
            StartTime = bookingViewModel.StartTime,
            EndTime = bookingViewModel.EndTime,
            Status = BookingStatus.Confirmed,
            CreatedAt = DateTime.UtcNow,
        };

        await _dbContext.Bookings.AddAsync(booking);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Booking created successfully for User {bookingViewModel.UserId}, Room {bookingViewModel.RoomId}");
    }

    public async Task<List<BookingViewModel>> GetUserBookingsAsync(Guid userId)
    {
        var bookings = await _dbContext.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .Where(b => b.UserId == userId)
            .Select(b => new BookingViewModel()
            {
                UserId = b.User.Id,
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