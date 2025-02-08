using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Roomly.Shared.Data.Entities;

namespace Roomly.Shared.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<AvailableSlot> AvailableSlots { get; set; }
    public DbSet<Entities.Booking> Bookings { get; set; }
}