using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBooking.MVC.Models
{
    public class CarDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        [Display(Name = "Brand")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [Display(Name = "Model")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [Display(Name = "Car Type")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Transmission type is required")]
        [Display(Name = "Transmission")]
        public string TransmissionType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fuel type is required")]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; } = string.Empty;

        [Required]
        [Range(1, 20, ErrorMessage = "Seating capacity must be between 1 and 20")]
        [Display(Name = "Seats")]
        public int SeatingCapacity { get; set; }

        [Required]
        [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price/Day")]
        public decimal PricePerDay { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; } = "USD";

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Range(1900, 2030, ErrorMessage = "Invalid year")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "License Plate")]
        public string? LicensePlate { get; set; }

        [Display(Name = "Color")]
        public string? Color { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        // For image upload
        [Display(Name = "Car Image")]
        public IFormFile? Image { get; set; }
    }
}
