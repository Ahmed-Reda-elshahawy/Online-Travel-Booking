using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBooking.MVC.Models;

public record LoginViewModel
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}