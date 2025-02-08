using Microsoft.AspNetCore.Mvc;
using Roomly.Rooms.Services;
using Roomly.Rooms.ViewModels;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Rooms.Controllers;

[ApiController]
[Route("/api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(RoomViewModel roomViewModel)
    {
        try
        {
            await _roomService.CreateRoomAsync(roomViewModel);
            
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
    
    [HttpGet]
    public async Task<IActionResult> GetAvailableRooms()
    {
        var rooms = await _roomService.GetAvailableRoomsAsync();
        
        return Ok(rooms);
    }
    
    [HttpGet("{id:guid}/slots")]
    public async Task<IActionResult> GetAvailableSlots(Guid id)
    {
        var rooms = await _roomService.GetAvailableSlotsByRoomIdAsync(id);
        
        return Ok(rooms);
    }
}