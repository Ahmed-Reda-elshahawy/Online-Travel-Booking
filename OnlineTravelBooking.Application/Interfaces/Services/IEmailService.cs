namespace OnlineTravelBooking.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
    Task SendPasswordResetOtpAsync(string toEmail, string userName, string otp);
    Task SendEmailVerificationOtpAsync(string toEmail, string userName, string otp);
}
