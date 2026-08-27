using KernalTravelGuide.Data;
using KernalTravelGuide.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Booking
    {
        public int Id { get; set; }


        // =========================
        // CUSTOMER
        // =========================

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }


        // =========================
        // BOOKING TYPE
        // =========================

        [Required]
        public BookingType BookingType { get; set; }


        // =========================
        // BOOKABLE ITEMS
        // =========================

        public int? TourPackageId { get; set; }

        public TourPackage? TourPackage { get; set; }


        public int? HotelId { get; set; }

        public Hotel? Hotel { get; set; }


        public int? ResortId { get; set; }

        public Resort? Resort { get; set; }


        public int? RestaurantId { get; set; }

        public Restaurant? Restaurant { get; set; }


        public int? TouristSpotId { get; set; }

        public TouristSpot? TouristSpot { get; set; }


        public int? TravelInformationId { get; set; }

        public TravelInformation? TravelInformation { get; set; }

        // BOOKING DATE / TRAVEL DATE


        [Required]
        [DataType(DataType.Date)]
        public DateTime TravelDate { get; set; }
      
  


        [Required]
        [Range(1, 20)]
        public int NumberOfPersons { get; set; }


        // HOTEL / RESORT


        [Range(1, 30)]
        public int? NumberOfNights { get; set; }


        [Range(1, 20)]
        public int? RoomsCount { get; set; }


        [Required]
        [Range(typeof(double), "0", "999999999")]
        public double TotalAmount { get; set; }

     

        [Required]
        public BookingStatus Status { get; set; }
            = BookingStatus.Pending;



        [Required]
        public DateTime BookingDate { get; set; }
            = DateTime.Now;
    }
}