using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Application.ViewModels;
using OnlineTravelBooking.Application.ViewModels.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<ResultViewModel<PaymentIntentViewModel>> CreatePaymentIntentAsync(CreatePaymentRequest request, CancellationToken ct = default);
        Task<ResultViewModel<PaymentConfirmationViewModel>> ConfirmPaymentAsync(string paymentIntentId, CancellationToken ct = default);
        Task<ResultViewModel<RefundViewModel>> RefundPaymentAsync(RefundRequest request, CancellationToken ct = default);
        Task<ResultViewModel<PaymentStatusViewModel>> GetPaymentStatusAsync(string paymentIntentId, CancellationToken ct = default);
    }
}
