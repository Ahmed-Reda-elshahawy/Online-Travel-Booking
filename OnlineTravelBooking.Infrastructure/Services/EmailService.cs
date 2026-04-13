using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Services;

namespace OnlineTravelBooking.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings emailSettings;
    public EmailService(IOptions<EmailSettings> emailSettingsOptions)
    {
        this.emailSettings = emailSettingsOptions.Value;
    }


    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        if (isHtml)
        {
            bodyBuilder.HtmlBody = body;
        }
        else
        {
            bodyBuilder.TextBody = body;
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

        try
        {
            // Connect to SMTP server
            await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, emailSettings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

            // Authenticate if credentials provided
            if (!string.IsNullOrEmpty(emailSettings.Username) && !string.IsNullOrEmpty(emailSettings.Password))
            {
                await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password);
            }

            // Send email
            await client.SendAsync(message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    public async Task SendPasswordResetOtpAsync(string toEmail, string userName, string otp)
    {
        var subject = "Password Reset OTP - Travel Booking";

        var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ background-color: #f9f9f9; padding: 30px; border-radius: 0 0 5px 5px; }}
                    .otp-box {{ background-color: #fff; border: 2px dashed #4CAF50; padding: 20px; text-align: center; margin: 20px 0; border-radius: 5px; }}
                    .otp-code {{ font-size: 32px; font-weight: bold; color: #4CAF50; letter-spacing: 5px; }}
                    .footer {{ text-align: center; margin-top: 20px; color: #777; font-size: 12px; }}
                    .warning {{ color: #d32f2f; margin-top: 15px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Password Reset Request</h1>
                    </div>
                    <div class='content'>
                        <p>Hello <strong>{userName}</strong>,</p>
                        <p>We received a request to reset your password for your Online Travel Booking account.</p>
                        <p>Your One-Time Password (OTP) is:</p>
                        <div class='otp-box'>
                            <div class='otp-code'>{otp}</div>
                        </div>
                        <p>This OTP will expire in <strong>15 minutes</strong>.</p>
                        <p class='warning'>⚠️ If you did not request this password reset, please ignore this email or contact support if you have concerns.</p>
                        <p>For security reasons, never share this OTP with anyone.</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2026 Travel Booking. All rights reserved.</p>
                        <p>This is an automated message, please do not reply to this email.</p>
                    </div>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(toEmail, subject, body, true);
    }

    public async Task SendEmailVerificationOtpAsync(string toEmail, string userName, string otp)
    {
        var subject = "Email Verification - Travel Booking";

        var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ background-color: #f9f9f9; padding: 30px; border-radius: 0 0 5px 5px; }}
                    .otp-box {{ background-color: #fff; border: 2px dashed #2196F3; padding: 20px; text-align: center; margin: 20px 0; border-radius: 5px; }}
                    .otp-code {{ font-size: 32px; font-weight: bold; color: #2196F3; letter-spacing: 5px; }}
                    .footer {{ text-align: center; margin-top: 20px; color: #777; font-size: 12px; }}
                    .warning {{ color: #d32f2f; margin-top: 15px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Welcome to Travel Booking!</h1>
                    </div>
                    <div class='content'>
                        <p>Hello <strong>{userName}</strong>,</p>
                        <p>Thank you for registering with Online Travel Booking. Please verify your email address to activate your account.</p>
                        <p>Your verification code is:</p>
                        <div class='otp-box'>
                            <div class='otp-code'>{otp}</div>
                        </div>
                        <p>This code will expire in <strong>15 minutes</strong>.</p>
                        <p>Please enter this code on the verification page to complete your registration.</p>
                        <p class='warning'>⚠️ If you did not create an account, please ignore this email.</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2026 Travel Booking. All rights reserved.</p>
                        <p>This is an automated message, please do not reply to this email.</p>
                    </div>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(toEmail, subject, body, true);
    }
}
