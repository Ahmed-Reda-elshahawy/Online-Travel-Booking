using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.Features.BookCar.Commands.CreateCar;
using OnlineTravelBooking.Application.Features.BookCar.Commands.DeleteCar;
using OnlineTravelBooking.Application.Features.BookCar.Commands.UpdateCar;
using OnlineTravelBooking.Application.Features.BookCar.Queries.GetAllCars;
using OnlineTravelBooking.Application.Features.BookCar.Queries.GetCarById;
using OnlineTravelBooking.Application.Features.BookCar.Queries.GetCarsByBrand;
using OnlineTravelBooking.Application.Features.BookCar.Queries.SearchCars;
[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllCarsQuery(pageNumber, pageSize));
        return StatusCode(result.StatusCode, result);
    }

    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCarById(int id)
    {
        var result = await _mediator.Send(new GetCarByIdQuery(id));
        return StatusCode(result.StatusCode, result);
    }

    
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCars([FromBody] CarSearchDto searchDto)
    {
        var result = await _mediator.Send(new SearchCarsQuery(searchDto));
        return StatusCode(result.StatusCode, result);
    }

  
    [HttpGet("brand/{brand}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCarsByBrand(
        string brand,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetCarsByBrandQuery(brand, pageNumber, pageSize));
        return StatusCode(result.StatusCode, result);
    }

 
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarDto carDto)
    {
        var result = await _mediator.Send(new CreateCarCommand(carDto));

        if (result.Success && result.Data != null)
        {
            return CreatedAtAction(
                nameof(GetCarById),
                new { id = result.Data.Id },
                result
            );
        }

        return StatusCode(result.StatusCode, result);
    }

    
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDto carDto)
    {
        var result = await _mediator.Send(new UpdateCarCommand(id, carDto));
        return StatusCode(result.StatusCode, result);
    }

    
    [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var result = await _mediator.Send(new DeleteCarCommand(id));
        return StatusCode(result.StatusCode, result);
    }
}