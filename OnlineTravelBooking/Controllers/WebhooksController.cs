

// API/Controllers/WebhooksController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using OnlineTravelBooking.Infrastructure.Services;
using OnlineTravelBooking.Infrastructure.Persistence;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using Microsoft.EntityFrameworkCore;

namespace OnlineTravelBooking.API.Controllers;

[ApiController]
[Route("api/v1/webhooks")]
public class WebhooksController : ControllerBase
{
    private readonly StripeSettings _stripeSettings;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WebhooksController> _logger;

    public WebhooksController(
        IOptions<StripeSettings> stripeSettings,
        ApplicationDbContext context,
        ILogger<WebhooksController> logger)
    {
        _stripeSettings = stripeSettings.Value;
        _context = context;
        _logger = logger;
    }

   
    [HttpPost("stripe")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _stripeSettings.WebhookSecret
            );

            _logger.LogInformation("Stripe webhook received: {EventType}", stripeEvent.Type);

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await HandlePaymentSucceeded(stripeEvent);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    await HandlePaymentFailed(stripeEvent);
                    break;

                case EventTypes.ChargeRefunded:
                    await HandleChargeRefunded(stripeEvent);
                    break;

                default:
                    _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                    break;
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe webhook error");
            return BadRequest();
        }
    }

    private async Task HandlePaymentSucceeded(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        var payment = await _context.Payments
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntent.Id);

        if (payment != null && payment.Status != PaymentStatus.Paid)
        {
            payment.Status = PaymentStatus.Paid;
            payment.TransactionId = paymentIntent.LatestChargeId;
            payment.PaidAt = DateTime.UtcNow;

            payment.Booking.PaymentStatus = PaymentStatus.Paid;
            payment.Booking.Status = BookingStatus.Confirmed;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Payment succeeded for booking {BookingId}, payment intent {PaymentIntentId}",
                payment.BookingId,
                paymentIntent.Id
            );
        }
    }

    private async Task HandlePaymentFailed(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        var payment = await _context.Payments
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntent.Id);

        if (payment != null)
        {
            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = paymentIntent.LastPaymentError?.Message;

            payment.Booking.PaymentStatus = PaymentStatus.Failed;

            await _context.SaveChangesAsync();

            _logger.LogWarning(
                "Payment failed for booking {BookingId}: {Reason}",
                payment.BookingId,
                payment.FailureReason
            );
        }
    }

    private async Task HandleChargeRefunded(Event stripeEvent)
    {
        var charge = stripeEvent.Data.Object as Charge;
        if (charge == null) return;

        var payment = await _context.Payments
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.TransactionId == charge.Id);

        if (payment != null && payment.Status != PaymentStatus.Refunded)
        {
            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = DateTime.UtcNow;

            payment.Booking.PaymentStatus = PaymentStatus.Refunded;
            payment.Booking.Status = BookingStatus.Cancelled;
            payment.Booking.RefundProcessedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Refund processed for booking {BookingId}",
                payment.BookingId
            );
        }
    }
}

