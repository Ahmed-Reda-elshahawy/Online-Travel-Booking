namespace OnlineTravelBooking.Application.DTOs.Auth;

public record UserDto(int Id, string FirstName, string LastName, string? Email, string? Token, string? RefreshToken = null);
