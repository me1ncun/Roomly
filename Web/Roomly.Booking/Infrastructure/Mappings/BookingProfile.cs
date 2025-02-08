using AutoMapper;
using Roomly.Booking.ViewModels;

namespace Roomly.Booking.Mappings;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<BookingViewModel, Shared.Data.Entities.Booking>();
    }
}