using Microsoft.EntityFrameworkCore;
using Roomly.Shared.Data.Entities;

namespace Roomly.Shared.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<AvailableSlot> AvailableSlots { get; set; }
    public DbSet<Booking> Bookings { get; set; }
}