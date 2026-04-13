using FluentValidation;
using MediatR;

namespace OnlineTravelBooking.Application.Features.Auth.Commands.VerifyEmail;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required.")
            .Length(6).WithMessage("OTP must be 6 characters long.")
            .Matches("^[0-9]+$").WithMessage("OTP must contain only numbers.");
    }
}
