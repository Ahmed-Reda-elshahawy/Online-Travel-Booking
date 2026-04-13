using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Application.DTOs.Payment;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Application.ViewModels;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities;
using OnlineTravelBooking.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Infrastructure.Services
{
    public class PaymentApplicationService: IPaymentApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public PaymentApplicationService(
            ApplicationDbContext context,
            IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;

        }
        public async Task<ResultViewModel<CreatePaymentResponseDto>> CreatePaymentAsync(
              CreatePaymentDto dto,
              CancellationToken ct = default)
        {
            
            var booking = await _context.bookings
                .FirstOrDefaultAsync(b => b.Id == dto.BookingId && b.UserId == dto.UserId, ct);

            if (booking == null)
            {
                return ResultViewModel<CreatePaymentResponseDto>.Fail("Booking not found or unauthorized");
            }

            if (booking.PaymentStatus == PaymentStatus.Paid)
            {
                return ResultViewModel<CreatePaymentResponseDto>.Fail("Booking is already paid");
            }

           
            var paymentRequest = new CreatePaymentRequest(
                booking.Id,
                booking.TotalPrice,
                booking.Currency,
                new Dictionary<string, string>
                {
                { "booking_category", booking.Category.ToString() },
                { "user_id", dto.UserId.ToString() }
                }
            );

            var paymentResult = await _paymentService.CreatePaymentIntentAsync(paymentRequest, ct);

            if (!paymentResult.Success)
            {
                return ResultViewModel<CreatePaymentResponseDto>.Fail(paymentResult.ErrorMessage!);
            }

         
            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = booking.TotalPrice,
                Currency = booking.Currency,
                Gateway = "Stripe",
                Status = PaymentStatus.Pending,
                PaymentIntentId = paymentResult.Data!.PaymentIntentId,
                ClientSecret = paymentResult.Data.ClientSecret,
                ResponseJson = JsonSerializer.Serialize(paymentResult.Data),
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            
            booking.PaymentMethod = dto.PaymentMethod;
            booking.PaymentStatus = PaymentStatus.Pending;

            await _context.SaveChangesAsync(ct);

            var response = new CreatePaymentResponseDto(
                payment.Id,
                payment.PaymentIntentId!,
                payment.ClientSecret!,
                payment.Amount,
                payment.Currency,
                payment.Status.ToString()
            );

            return ResultViewModel<CreatePaymentResponseDto>.Ok(response);
        }
        public async Task<ResultViewModel<ConfirmPaymentResponseDto>> ConfirmPaymentAsync( ConfirmPaymentDto dto, CancellationToken ct = default)
        {
            var bookingData = await _context.bookings
                 .Where(b => b.Id == dto.BookingId)
                 .Select(b => new
                 {
                     b.Id,
                     b.PaymentStatus,
                     b.Status,
                     b.TotalPrice,
                     b.Currency,
                     b.PaymentMethod,
                     b.Category,
                     Review = b.review != null ? new
                     {
                         b.review.Id,
                         b.review.Rating,
                         b.review.CreatedAt
                     } : null
                 })
                 .FirstOrDefaultAsync(ct);

            if (bookingData == null)
            {
                return ResultViewModel<ConfirmPaymentResponseDto>.Fail("Booking not found");
            }

            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == dto.BookingId &&
                                         p.PaymentIntentId == dto.PaymentIntentId, ct);

            if (payment == null)
            {
                return ResultViewModel<ConfirmPaymentResponseDto>.Fail("Payment record not found");
            }

            var confirmResult = await _paymentService.ConfirmPaymentAsync(dto.PaymentIntentId, ct);

            if (!confirmResult.Success)
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = confirmResult.ErrorMessage;

                await _context.SaveChangesAsync(ct);

                return ResultViewModel<ConfirmPaymentResponseDto>.Fail(confirmResult.ErrorMessage!);
            }

            var confirmation = confirmResult.Data!;

            var booking = await _context.bookings.FindAsync(new object[] { dto.BookingId }, cancellationToken: ct);

            if (booking == null)
            {
                return ResultViewModel<ConfirmPaymentResponseDto>.Fail("Booking not found for update");
            }

            if (confirmation.Status == "succeeded")
            {
                payment.Status = PaymentStatus.Paid;
                payment.TransactionId = confirmation.TransactionId;
                payment.PaidAt = confirmation.PaidAt ?? DateTime.UtcNow;
                payment.ResponseJson = JsonSerializer.Serialize(confirmation);

                booking.PaymentStatus = PaymentStatus.Paid;
                booking.Status = BookingStatus.Confirmed;
            }
            else if (confirmation.Status == "requires_payment_method" ||
                     confirmation.Status == "requires_confirmation")
            {
                payment.Status = PaymentStatus.Pending;
                booking.PaymentStatus = PaymentStatus.Pending;
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                booking.PaymentStatus = PaymentStatus.Failed;
            }

            await _context.SaveChangesAsync(ct);

            var response = new ConfirmPaymentResponseDto(
                confirmation.Status == "succeeded",
                booking.Status.ToString(),
                payment.Status.ToString(),
                payment.Amount,
                payment.PaidAt
            );

            return ResultViewModel<ConfirmPaymentResponseDto>.Ok(response);
        }
        public async Task<ResultViewModel<PaymentDetailsDto>> GetPaymentDetailsAsync(
              int bookingId,
              CancellationToken ct = default)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId, ct);

            if (payment == null)
            {
                return ResultViewModel<PaymentDetailsDto>.Fail("Payment not found");
            }

            var details = new PaymentDetailsDto(
                payment.Id,
                payment.BookingId,
                payment.Amount,
                payment.Currency,
                payment.Status.ToString(),
                payment.TransactionId,
                payment.PaidAt,
                payment.RefundedAt
            );

            return ResultViewModel<PaymentDetailsDto>.Ok(details);
        }
    }
}
