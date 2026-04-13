using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.DTOs.Payment;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Application.ViewModels;
using System.Security.Claims;

namespace OnlineTravelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentApplicationService _paymentService;
        public PaymentsController(IPaymentApplicationService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment(
       [FromBody] CreatePaymentDto dto,
       CancellationToken ct)
        {
            var userId = GetUserId();
            var dtoWithUser = dto with { UserId = userId };

            var result = await _paymentService.CreatePaymentAsync(dtoWithUser, ct);

            if (!result.Success)
            {
                return BadRequest(ResultViewModel<object>.Fail(result.ErrorMessage!));
            }

            return Ok(ResultViewModel<CreatePaymentResponseDto>.Ok(result.Data!));
        }
       
        
        [HttpPost("confirm")]
        [ProducesResponseType(typeof(ResultViewModel<ConfirmPaymentResponseDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConfirmPayment(
           [FromBody] ConfirmPaymentDto dto,
           CancellationToken ct)
        {
            var result = await _paymentService.ConfirmPaymentAsync(dto, ct);

            if (!result.Success)
            {
                return BadRequest(ResultViewModel<object>.Fail(result.ErrorMessage!));
            }

            return Ok(ResultViewModel<ConfirmPaymentResponseDto>.Ok(result.Data!));
        }
      
        
        [HttpGet("booking/{bookingId}")]
        [ProducesResponseType(typeof(ResultViewModel<PaymentDetailsDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPaymentDetails(
               int bookingId,
               CancellationToken ct)
        {
            var result = await _paymentService.GetPaymentDetailsAsync(bookingId, ct);

            if (!result.Success)
            {
                return NotFound(ResultViewModel<object>.Fail(result.ErrorMessage!));
            }

            return Ok(ResultViewModel<PaymentDetailsDto>.Ok(result.Data!));
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

      
    }
}
