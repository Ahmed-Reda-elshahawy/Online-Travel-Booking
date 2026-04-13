using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.MVC.Models;
using OnlineTravelBooking.MVC.Services;

public class BookCarController : Controller
{
    private readonly ICarService _carService;
    private readonly IAttachmentService _attachmentService;
    private readonly IWebHostEnvironment _env;

    public BookCarController(
        ICarService carService,
        IAttachmentService attachmentService,
        IWebHostEnvironment env)
    {
        _carService = carService;
        _attachmentService = attachmentService;
        _env = env;
    }

    #region Index - List All Cars
    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllCarsAsync();

        var carDtos = new List<CarDto>();
        foreach (var car in cars)
        {
            carDtos.Add(new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Type = car.Type,
                TransmissionType = car.TransmissionType,
                FuelType = car.FuelType,
                SeatingCapacity = car.SeatingCapacity,
                PricePerDay = car.PricePerDay,
                Currency = car.Currency,
                ImageUrl = car.ImageUrl,
              
                Description = car.Description,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                Color = car.Color,
                CreatedAt = car.CreatedAt,
                UpdatedAt = car.UpdatedAt
            });
        }

        return View(carDtos);
    }
    #endregion

    #region Create Car
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CarDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CarDto carDto)
    {
        if (ModelState.IsValid)
        {
            // Handle image upload
            var imageUrl = string.Empty;
            if (carDto.Image != null)
            {
                imageUrl = _attachmentService.Upload(carDto.Image, "Images");
            }
            else if (!string.IsNullOrEmpty(carDto.ImageUrl))
            {
                imageUrl = carDto.ImageUrl;
            }

            var car = new Car
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                Type = carDto.Type,
                TransmissionType = carDto.TransmissionType,
                FuelType = carDto.FuelType,
                SeatingCapacity = carDto.SeatingCapacity,
                PricePerDay = carDto.PricePerDay,
                Currency = carDto.Currency,
                ImageUrl = imageUrl,
                Description = carDto.Description,
                Year = carDto.Year,
                LicensePlate = carDto.LicensePlate,
                Color = carDto.Color
            };

            var result = await _carService.CreateCarAsync(car);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Car created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create car. Please try again.";
            }
        }

        return View(carDto);
    }
    #endregion

    #region Edit Car
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);

        if (car == null)
        {
            return NotFound();
        }

        // Load existing image as FormFile if exists
        FormFile? imageFile = null;
        if (!string.IsNullOrEmpty(car.ImageUrl))
        {
            var path = Path.Combine(_env.WebRootPath, "Files", "Images", car.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                imageFile = new FormFile(
                    fileStream,
                    0,
                    fileStream.Length,
                    car.ImageUrl,
                    Path.GetFileName(path)
                );
            }
        }

        var carDto = new CarDto
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Type = car.Type,
            TransmissionType = car.TransmissionType,
            FuelType = car.FuelType,
            SeatingCapacity = car.SeatingCapacity,
            PricePerDay = car.PricePerDay,
            Currency = car.Currency,
            Image = imageFile,
            ImageUrl = car.ImageUrl,
       
            Description = car.Description,
            Year = car.Year,
            LicensePlate = car.LicensePlate,
            Color = car.Color
        };

        return View(carDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CarDto carDto)
    {
        if (ModelState.IsValid)
        {
            var existingCar = await _carService.GetCarByIdAsync(carDto.Id);

            if (existingCar == null)
            {
                return NotFound();
            }

            // Handle image upload
            if (carDto.Image != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(existingCar.ImageUrl))
                {
                    string existingFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot", "Files", "Images", existingCar.ImageUrl);

                    if (System.IO.File.Exists(existingFilePath))
                    {
                        _attachmentService.Delete(existingFilePath);
                    }
                }

                // Upload new image
                var newImageUrl = _attachmentService.Upload(carDto.Image, "Images");
                existingCar.ImageUrl = newImageUrl;
            }
            else if (!string.IsNullOrEmpty(carDto.ImageUrl))
            {
                existingCar.ImageUrl = carDto.ImageUrl;
            }

            // Update car properties
            existingCar.Brand = carDto.Brand;
            existingCar.Model = carDto.Model;
            existingCar.Type = carDto.Type;
            existingCar.TransmissionType = carDto.TransmissionType;
            existingCar.FuelType = carDto.FuelType;
            existingCar.SeatingCapacity = carDto.SeatingCapacity;
            existingCar.PricePerDay = carDto.PricePerDay;
            existingCar.Currency = carDto.Currency;
            existingCar.Description = carDto.Description;
            existingCar.Year = carDto.Year;
            existingCar.LicensePlate = carDto.LicensePlate;
            existingCar.Color = carDto.Color;

            var result = await _carService.UpdateCarAsync(existingCar);

            if (result > 0)
            {
                TempData["SuccessMessage"] = "Car updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update car. Please try again.";
            }
        }

        return View(carDto);
    }
    #endregion

    #region Delete Car
    [HttpPost]
    public async Task<IActionResult> Delete(int carId)
    {
        var car = await _carService.GetCarByIdAsync(carId);

        if (car == null)
        {
            return NotFound();
        }

        // Delete image file
        if (!string.IsNullOrEmpty(car.ImageUrl))
        {
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "Files", "Images", car.ImageUrl);

            if (System.IO.File.Exists(filePath))
            {
                _attachmentService.Delete(filePath);
            }
        }

        var result = await _carService.DeleteCarAsync(carId);

        if (result > 0)
        {
            TempData["SuccessMessage"] = "Car deleted successfully!";
        }
        else
        {
            return BadRequest();
        }

        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Details
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);

        if (car == null)
        {
            return NotFound();
        }

        var carDto = new CarDto
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Type = car.Type,
            TransmissionType = car.TransmissionType,
            FuelType = car.FuelType,
            SeatingCapacity = car.SeatingCapacity,
            PricePerDay = car.PricePerDay,
            Currency = car.Currency,
            ImageUrl = car.ImageUrl,
        
            Description = car.Description,
            Year = car.Year,
            LicensePlate = car.LicensePlate,
            Color = car.Color,
            CreatedAt = car.CreatedAt,
            UpdatedAt = car.UpdatedAt
        };

        return View(carDto);
    }
    #endregion

    #region Search by Brand
    [HttpGet]
    public async Task<IActionResult> SearchByBrand(string brand)
    {
        if (string.IsNullOrEmpty(brand))
        {
            return RedirectToAction(nameof(Index));
        }

        var cars = await _carService.SearchCarsByBrandAsync(brand);

        var carDtos = new List<CarDto>();
        foreach (var car in cars)
        {
            carDtos.Add(new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Type = car.Type,
                TransmissionType = car.TransmissionType,
                FuelType = car.FuelType,
                SeatingCapacity = car.SeatingCapacity,
                PricePerDay = car.PricePerDay,
                Currency = car.Currency,
                ImageUrl = car.ImageUrl,
               
                Description = car.Description,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                Color = car.Color,
                CreatedAt = car.CreatedAt,
                UpdatedAt = car.UpdatedAt
            });
        }

        ViewData["SearchBrand"] = brand;
        return View("Index", carDtos);
    }
    #endregion
}