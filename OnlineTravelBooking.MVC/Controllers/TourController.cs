using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Application.Services;
using OnlineTravelBooking.Domain.Entities.Tour;
using OnlineTravelBooking.MVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineTravelBooking.MVC.Controllers
{
    public class TourController : Controller
    {
        private readonly IGenericRepository<Tour> _repo;
        private readonly IAttachmentService _attachmentService;

        private readonly IWebHostEnvironment _env;

        public TourController(IGenericRepository<Tour> repo,IAttachmentService attachmentService, IWebHostEnvironment env)
        {
            _repo = repo;
            _attachmentService = attachmentService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var tours = await _repo.GetAll();

            var tourDtos = new List<TourDto>();

            foreach (var tour in tours)
            {
                var lang = tour.MeetingPoint?.Y;
                var lat = tour.MeetingPoint?.X;
                tourDtos.Add(new TourDto
                {
                    Id = tour.Id,
                    Title = tour.Title,
                    Slug = tour.Slug,
                    Description = tour.Description,
                    Summary = tour.Summary,
                    ImageUrl = tour.ImageUrl,
                    duration = tour.duration,
                    Latitude=lat,
                    Longitude=lang,
                    ProviderName = tour.ProviderName,
                    ProviderContact = tour.ProviderContact,
                    ViewCount = tour.ViewCount,
                    BookingCount = tour.BookingCount,
                    AverageRating = tour.AverageRating,
                    Price = tour.Price,
                    ReviewCount = tour.ReviewCount,
                    CategoryId = tour.CategoryId
                });
            }

            return View(tourDtos);
        }
        #region Delete Tour
        [HttpPost]
        public async Task<IActionResult> Delete(int tourId)
        {
            var tour = await _repo.GetById(tourId);
            if (tour == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(tour.ImageUrl))
            {
                string filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "Files", "Images", tour.ImageUrl);
                _attachmentService.Delete(filePath);
            }
            var res= await _repo.Delete(tourId);
            if(res==0)
                return BadRequest();
            else
                return RedirectToAction("Index");
        }
        #endregion
        #region AddTour
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TourDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TourDto tourDto)
        {
            if (ModelState.IsValid)
            {
                var imageUrl = _attachmentService.Upload(tourDto.Image, "Images");
                Point? meetingPoint = null;

                if (tourDto.Latitude.HasValue && tourDto.Longitude.HasValue)
                {
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                    meetingPoint = geometryFactory.CreatePoint(
                        new Coordinate(tourDto.Longitude.Value, tourDto.Latitude.Value)
                    );
                }

                var tour = new Tour
                {
                    Title = tourDto.Title,
                    Slug = tourDto.Slug,
                    Description = tourDto.Description,
                    Summary = tourDto.Summary,
                    ImageUrl = imageUrl,
                    duration = tourDto.duration,
                    MeetingPoint = meetingPoint,
                    ProviderName = tourDto.ProviderName,
                    ProviderContact = tourDto.ProviderContact,
                    ViewCount = tourDto.ViewCount,
                    BookingCount = tourDto.BookingCount,
                    AverageRating = tourDto.AverageRating,
                    Price = tourDto.Price,
                    ReviewCount = tourDto.ReviewCount,
                    CategoryId = tourDto.CategoryId
                };
                await _repo.Add(tour);
                return RedirectToAction("Index");
            }

            return View(tourDto);
        }
        #endregion
        #region Edit Tour
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tour = await _repo.GetById(id);
            if (tour == null)
            {
                return NotFound();
            }

            FormFile? imageFile = null;

            if (!string.IsNullOrEmpty(tour.ImageUrl))
            {
                var path = Path.Combine(_env.WebRootPath, "Files", "Images", tour.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    imageFile = new FormFile(fileStream, 0, fileStream.Length, tour.ImageUrl ,Path.GetFileName(path));
                }
            }



            double? latitude = null;
            double? longitude = null;
            if (tour.MeetingPoint != null)
            {
                latitude = tour.MeetingPoint.X;
                longitude = tour.MeetingPoint.Y;
            }
            var tourDto = new TourDto
            {
                Id = tour.Id,
                Title = tour.Title,
                Slug = tour.Slug,
                Description = tour.Description,
                Summary = tour.Summary,
                Image = imageFile,
                ImageUrl = tour.ImageUrl,
                duration = tour.duration,
                Latitude = latitude,
                Longitude = longitude,
                ProviderName = tour.ProviderName,
                ProviderContact = tour.ProviderContact,
                ViewCount = tour.ViewCount,
                BookingCount = tour.BookingCount,
                AverageRating = tour.AverageRating,
                Price = tour.Price,
                ReviewCount = tour.ReviewCount,
                CategoryId = tour.CategoryId
            };
            return View(tourDto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TourDto tourDto)
        {
            if (ModelState.IsValid)
            {
                var existingTour = await _repo.GetById(tourDto.Id);
                if (existingTour == null)
                {
                    return NotFound();
                }
                Point? meetingPoint = null;
                if (tourDto.Latitude.HasValue && tourDto.Longitude.HasValue)
                {
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                    meetingPoint = geometryFactory.CreatePoint(
                        new Coordinate(tourDto.Longitude.Value, tourDto.Latitude.Value)
                    );
                }

                if(tourDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(existingTour.ImageUrl))
                    {
                        string existingFilePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot", "Files", "Images", existingTour.ImageUrl);
                        if(System.IO.File.Exists(existingFilePath))
                            _attachmentService.Delete(existingFilePath);
                    }
                    var newImageUrl = _attachmentService.Upload(tourDto.Image, "Images");
                    existingTour.ImageUrl = newImageUrl;
                }


                existingTour.Id = tourDto.Id;
                existingTour.Title = tourDto.Title;
                existingTour.Slug = tourDto.Slug;
                existingTour.Description = tourDto.Description;
                existingTour.Summary = tourDto.Summary;
                existingTour.duration = tourDto.duration;
                existingTour.MeetingPoint = meetingPoint;
                existingTour.ProviderName = tourDto.ProviderName;
                existingTour.ProviderContact = tourDto.ProviderContact;
                existingTour.ViewCount = tourDto.ViewCount;
                existingTour.BookingCount = tourDto.BookingCount;
                existingTour.AverageRating = tourDto.AverageRating;
                existingTour.Price = tourDto.Price;
                existingTour.ReviewCount = tourDto.ReviewCount;
                existingTour.CategoryId = tourDto.CategoryId;
                await _repo.Update(existingTour);
                return RedirectToAction("Index");
            }
            return View(tourDto);
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tour = await _repo.GetById(id);
            if (tour == null)
            {
                return NotFound();
            }
            double? latitude = null;
            double? longitude = null;
            if (tour.MeetingPoint != null)
            {
                latitude = tour.MeetingPoint.X;
                longitude = tour.MeetingPoint.Y;
            }
            var tourDto = new TourDto
            {
                Id = tour.Id,
                Title = tour.Title,
                Slug = tour.Slug,
                Description = tour.Description,
                Summary = tour.Summary,
                ImageUrl = tour.ImageUrl,
                duration = tour.duration,
                Latitude = latitude,
                Longitude = longitude,
                ProviderName = tour.ProviderName,
                ProviderContact = tour.ProviderContact,
                ViewCount = tour.ViewCount,
                BookingCount = tour.BookingCount,
                AverageRating = tour.AverageRating,
                Price = tour.Price,
                ReviewCount = tour.ReviewCount,
                CategoryId = tour.CategoryId
            };
            return View(tourDto);
        }
        #endregion
    }
}
