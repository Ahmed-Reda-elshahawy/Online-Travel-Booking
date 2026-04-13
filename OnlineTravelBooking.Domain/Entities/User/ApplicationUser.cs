using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Domain.Entities.User;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
    public string? ProfileImageUrl { get; set; }
    // Initialize Location with a valid geography point (SRID 4326) to avoid invalid geography parameter errors
    public Point? Location { get; set; } = new Point(new Coordinate(0, 0)) { SRID = 4326 };
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Refresh token properties (stored on user)
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    // OTP properties for password reset
    public string? PasswordResetOtp { get; set; }
    public DateTime? PasswordResetOtpExpiry { get; set; }

    // OTP properties for email verification
    public string? EmailVerificationOtp { get; set; }
    public DateTime? EmailVerificationOtpExpiry { get; set; }
    public bool IsEmailVerified { get; set; } = false;

    // Navigation properties
    public ICollection<BaseBooking> Bookings { get; set; } = new HashSet<BaseBooking>();
}
