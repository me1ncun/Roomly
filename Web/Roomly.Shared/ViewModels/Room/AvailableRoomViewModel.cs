namespace Roomly.Rooms.ViewModels;

public class AvailableRoomViewModel
{
    public string RoomName { get; set; }
    public string RoomDescription { get; set; }
    public string RoomLocation { get; set; }
    public int RoomCapacity { get; set; }
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
}