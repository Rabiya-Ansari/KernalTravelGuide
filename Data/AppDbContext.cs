using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext
    : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<TouristSpot> TouristSpots => Set<TouristSpot>();
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Resort> Resorts => Set<Resort>();
    public DbSet<TravelInformation> TravelInformations => Set<TravelInformation>();
    public DbSet<TourPackage> TourPackages => Set<TourPackage>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Gallery> Galleries => Set<Gallery>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TravelInformation>()
            .HasOne(t => t.FromCity)
            .WithMany()
            .HasForeignKey(t => t.FromCityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TravelInformation>()
            .HasOne(t => t.ToCity)
            .WithMany()
            .HasForeignKey(t => t.ToCityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
