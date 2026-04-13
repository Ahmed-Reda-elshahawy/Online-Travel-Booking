using Microsoft.Extensions.Options;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Application.ViewModels;
using OnlineTravelBooking.Application.ViewModels.Payment;
using OnlineTravelBooking.Infrastructure.Persistence;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace OnlineTravelBooking.Infrastructure.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly StripeSettings _settings;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly RefundService _refundService;
        public PaymentService(IOptions<StripeSettings> options)
        {
            _settings = options.Value;
            StripeConfiguration.ApiKey = _settings.SecretKey;
            _paymentIntentService = new PaymentIntentService();
            _refundService = new RefundService();
        }
        public async Task<ResultViewModel<PaymentIntentViewModel>> CreatePaymentIntentAsync(
         CreatePaymentRequest request,
         CancellationToken ct = default)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(request.Amount * 100),
                    Currency = request.Currency.ToLower(),
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                        AllowRedirects = "never"
                    },
                    Metadata = request.Metadata ?? new Dictionary<string, string>()
                };

                options.Metadata["booking_id"] = request.BookingId.ToString();

                var paymentIntent = await _paymentIntentService.CreateAsync(options, cancellationToken: ct);

                var response = new PaymentIntentViewModel(
                    paymentIntent.Id,
                    paymentIntent.ClientSecret,
                    request.Amount,
                    request.Currency,
                    paymentIntent.Status
                );


                return ResultViewModel<PaymentIntentViewModel>.Ok(response);
            }
            catch (StripeException ex)
            {
                return ResultViewModel<PaymentIntentViewModel>.Fail($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ResultViewModel<PaymentIntentViewModel>.Fail($"Payment creation failed: {ex.Message}");
            }
        }
        public async Task<ResultViewModel<PaymentConfirmationViewModel>> ConfirmPaymentAsync(
        string paymentIntentId,
        CancellationToken ct = default)
        {

            try
            {
                var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId, cancellationToken: ct);

                if (paymentIntent == null)
                {
                    return ResultViewModel<PaymentConfirmationViewModel>.Fail("Payment intent not found");
                }

                var response = new PaymentConfirmationViewModel(
                    paymentIntent.Id,
                    paymentIntent.Status,
                    paymentIntent.Amount / 100m, 
                    paymentIntent.Status == "succeeded" ? DateTime.UtcNow : null,
                    paymentIntent.LatestChargeId
                );

                return ResultViewModel<PaymentConfirmationViewModel>.Ok(response);
            }
            catch (StripeException ex)
            {
                return ResultViewModel<PaymentConfirmationViewModel>.Fail($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ResultViewModel<PaymentConfirmationViewModel>.Fail($"Payment confirmation failed: {ex.Message}");
            }
        }

        public async Task<ResultViewModel<RefundViewModel>> RefundPaymentAsync(
       RefundRequest request,
       CancellationToken ct = default)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = request.PaymentIntentId,
                    Reason = request.Reason switch
                    {
                        "duplicate" => "duplicate",
                        "fraudulent" => "fraudulent",
                        _ => "requested_by_customer"
                    },
                    Metadata = new Dictionary<string, string>
                {
                    { "booking_id", request.BookingId.ToString() }
                }
                };

                // Partial refund if amount specified
                if (request.Amount.HasValue)
                {
                    options.Amount = (long)(request.Amount.Value * 100);
                }

                var refund = await _refundService.CreateAsync(options, cancellationToken: ct);

                var response = new RefundViewModel(
                    refund.Id,
                    refund.Amount / 100m,
                    refund.Status,
                    DateTime.UtcNow
                );

                return ResultViewModel<RefundViewModel>.Ok(response);
            }
            catch (StripeException ex)
            {
                return ResultViewModel<RefundViewModel>.Fail($"Stripe refund error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ResultViewModel<RefundViewModel>.Fail($"Refund failed: {ex.Message}");
            }
        }


        public async Task<ResultViewModel<PaymentStatusViewModel>> GetPaymentStatusAsync(
       string paymentIntentId,
       CancellationToken ct = default)
        {
            try
            {
                var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId, cancellationToken: ct);

                if (paymentIntent == null)
                {
                    return ResultViewModel<PaymentStatusViewModel>.Fail("Payment intent not found");
                }

                var response = new PaymentStatusViewModel(
                    paymentIntent.Id,
                    paymentIntent.Status,
                    paymentIntent.Amount / 100m,
                    paymentIntent.Currency.ToUpper(),
                    paymentIntent.Status == "succeeded" ? DateTime.UtcNow : null
                );

                return ResultViewModel<PaymentStatusViewModel>.Ok(response);
            }
            catch (StripeException ex)
            {
                return ResultViewModel<PaymentStatusViewModel>.Fail($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return ResultViewModel<PaymentStatusViewModel>.Fail($"Status check failed: {ex.Message}");
            }
        }

    }
}
