using Roomly.Shared.Data.Enums;

namespace Roomly.Booking.ViewModels;

public class BookingViewModel
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string RoomName { get; set; }
    public string RoomLocation { get; set; }
    public int RoomCapacity { get; set; }
    public RoomType RoomType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}