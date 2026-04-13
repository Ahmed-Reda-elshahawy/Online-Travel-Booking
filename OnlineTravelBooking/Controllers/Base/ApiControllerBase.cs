using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value == null ? NoContent() : Ok(result.Value);
        }

        return result.Error.Code switch
        {
            "NotFound" => NotFound(new { error = result.Error.Message }),
            "Unauthorized" => Unauthorized(new { error = result.Error.Message }),
            "Forbidden" => Forbid(),
            "Validation" => BadRequest(new { error = result.Error.Message }),
            _ => BadRequest(new { error = result.Error.Message })
        };
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return result == null ? NoContent() : Ok(result);
        }

        return result.Error.Code switch
        {
            "NotFound" => NotFound(new { error = result.Error.Message }),
            "Unauthorized" => Unauthorized(new { error = result.Error.Message }),
            "Forbidden" => Forbid(),
            "Validation" => BadRequest(new { error = result.Error.Message }),
            _ => BadRequest(new { error = result.Error.Message })
        };
    }
}
