using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Payment;
using OnlineTravelBooking.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Interfaces.Services
{
    public interface IPaymentApplicationService
    {
        Task<ResultViewModel<CreatePaymentResponseDto>> CreatePaymentAsync(CreatePaymentDto dto, CancellationToken ct = default);
        Task<ResultViewModel<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(ConfirmPaymentDto dto, CancellationToken ct = default);
        //Task<ResultViewModel<RefundResponseDto>> ProcessRefundAsync(ProcessRefundDto dto, CancellationToken ct = default);
        Task<ResultViewModel<PaymentDetailsDto>> GetPaymentDetailsAsync(int bookingId, CancellationToken ct = default);
    }
}
