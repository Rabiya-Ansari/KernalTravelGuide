using KernalTravelGuide.Data;
using KernalTravelGuide.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KernalTravelGuide.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        [Required]
        public int TourPackageId { get; set; }

        public TourPackage? TourPackage { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TravelDate { get; set; }

        [Required]
        [Range(1, 20)]
        public int NumberOfPersons { get; set; }

        [Required]
        [Range(typeof(double), "1", "999999")]
        public double TotalAmount { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}
