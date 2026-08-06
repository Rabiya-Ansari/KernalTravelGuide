using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class PackageBooking
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        public Booking? Booking { get; set; }

        [Required]
        public int TourPackageId { get; set; }

        public TourPackage? TourPackage { get; set; }

        [Required]
        [Range(1, 20)]
        public int Persons { get; set; }

        [Range(typeof(double), "1", "999999")]
        public double Amount { get; set; }
    }
}
