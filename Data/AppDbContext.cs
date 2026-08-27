using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
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
    // REMOVED: DbSet<Review> line deleted

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // TravelInformation: FromCity
        builder.Entity<TravelInformation>()
            .HasOne(t => t.FromCity)
            .WithMany()
            .HasForeignKey(t => t.FromCityId)
            .OnDelete(DeleteBehavior.Restrict);

        // TravelInformation: ToCity
        builder.Entity<TravelInformation>()
            .HasOne(t => t.ToCity)
            .WithMany()
            .HasForeignKey(t => t.ToCityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Feedback: Hotel
        builder.Entity<Feedback>()
            .HasOne(f => f.Hotel)
            .WithMany()
            .HasForeignKey(f => f.HotelId)
            .OnDelete(DeleteBehavior.NoAction);

        // Feedback: Resort
        builder.Entity<Feedback>()
            .HasOne(f => f.Resort)
            .WithMany()
            .HasForeignKey(f => f.ResortId)
            .OnDelete(DeleteBehavior.NoAction);

        // Feedback: Restaurant
        builder.Entity<Feedback>()
            .HasOne(f => f.Restaurant)
            .WithMany()
            .HasForeignKey(f => f.RestaurantId)
            .OnDelete(DeleteBehavior.NoAction);

        // Feedback: Tourist Spot
        builder.Entity<Feedback>()
            .HasOne(f => f.TouristSpot)
            .WithMany()
            .HasForeignKey(f => f.TouristSpotId)
            .OnDelete(DeleteBehavior.NoAction);

        // Feedback: Tour Package
        builder.Entity<Feedback>()
            .HasOne(f => f.TourPackage)
            .WithMany()
            .HasForeignKey(f => f.TourPackageId)
            .OnDelete(DeleteBehavior.NoAction);
        // ============================
        // Booking relationships
        // ============================

        builder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Booking>()
            .HasOne(b => b.TourPackage)
            .WithMany()
            .HasForeignKey(b => b.TourPackageId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Booking>()
            .HasOne(b => b.Hotel)
            .WithMany()
            .HasForeignKey(b => b.HotelId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Booking>()
            .HasOne(b => b.Resort)
            .WithMany()
            .HasForeignKey(b => b.ResortId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Booking>()
            .HasOne(b => b.Restaurant)
            .WithMany()
            .HasForeignKey(b => b.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Booking>()
            .HasOne(b => b.TouristSpot)
            .WithMany()
            .HasForeignKey(b => b.TouristSpotId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<Booking>()
            .HasOne(b => b.TravelInformation)
            .WithMany()
            .HasForeignKey(b => b.TravelInformationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}