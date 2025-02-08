using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Roomly.Rooms.ViewModels;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Rooms.Services;

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RoomService> _logger;
    private readonly IMapper _mapper;

    public RoomService(
        ApplicationDbContext dbContext,
        ILogger<RoomService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateRoomAsync(RoomViewModel roomViewModel)
    {
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(u => u.Name == roomViewModel.Name);
        if (room is not null)
        {
            throw new EntityAlreadyExistsException();
        }
        
        var roomEntity = _mapper.Map<Room>(roomViewModel);
        
        await _dbContext.Rooms.AddAsync(roomEntity);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Room {roomViewModel.Name} has been created");
    }

    public async Task<List<AvailableRoomViewModel>> GetAvailableRoomsAsync()
    {
        var rooms = await _dbContext.AvailableSlots
            .Include(s => s.Room)
            .Where(slot => slot.IsAvailable == true) 
            .Select(slot => new AvailableRoomViewModel()
            {
                RoomId = slot.RoomId,
                RoomName = slot.Room.Name,
                RoomDescription = slot.Room.Description,
                RoomLocation = slot.Room.Location,
                RoomCapacity = slot.Room.Capacity,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsAvailable = slot.IsAvailable
            })
            .ToListAsync();
        
        _logger.LogInformation($"Getted {rooms.Count} available rooms");
        
        return rooms;
    }

    public async Task<List<AvailableSlotViewModel>> GetAvailableSlotsByRoomIdAsync(Guid roomId)
    {
        var rooms = await _dbContext.AvailableSlots
            .Where(slot => slot.RoomId == roomId) 
            .Select(slot => new AvailableSlotViewModel
            {
                RoomId = slot.RoomId,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsAvailable = slot.IsAvailable
            })
            .ToListAsync();
        
        _logger.LogInformation($"Getted {rooms.Count} available slot rooms");
        
        return rooms;
    }
}

public interface IRoomService
{
    Task CreateRoomAsync(RoomViewModel roomViewModel);
    Task<List<AvailableRoomViewModel>> GetAvailableRoomsAsync();
    Task<List<AvailableSlotViewModel>> GetAvailableSlotsByRoomIdAsync(Guid roomId);
}