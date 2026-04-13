namespace OnlineTravelBooking.MVC.Models
{
    public class TourImagesDto
    {
        public int Id { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public int TourId { get; set; }
    }
}
