using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBooking.MVC.Models;

public class UpdateFlightCommandViewModel : CreateFlightCommandViewModel
{
    [Required]
    public int Id { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = string.Empty;
}