using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.Tour;
using OnlineTravelBooking.MVC.Models;
using System.Threading.Tasks;

namespace OnlineTravelBooking.MVC.Controllers
{
    public class TourImageController : Controller
    {
        private readonly IGenericRepository<TourImage> _tourImageRepository;
        private readonly IAttachmentService _attachmentService;


        private readonly IWebHostEnvironment _env;
        public TourImageController(IGenericRepository<TourImage> tourImageRepository,IAttachmentService attachmentService, IWebHostEnvironment env)
        {
            _tourImageRepository = tourImageRepository;
            _attachmentService = attachmentService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var tourImages = await _tourImageRepository.GetAll();
            var tourImageDtos = new List<TourImagesDto>();
            foreach (var image in tourImages)
            {
                tourImageDtos.Add(new TourImagesDto
                {
                    Id = image.Id,
                    TourId = image.TourId,
                    ImageFile = null, // ImageFile is not directly mapped; handle as needed
                    ImageUrl = image.ImageUrl
                });
            }
            return View(tourImageDtos);
        }
        #region DeleteImage
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var image = await _tourImageRepository.GetById(Id);
            var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "Files", "Images", image.ImageUrl);
            if (System.IO.File.Exists(filePath))
                _attachmentService.Delete(filePath);
            if (image == null)
            {
                return NotFound();
            }
            await _tourImageRepository.Delete(Id);
            return RedirectToAction("Index");
        }
        #endregion
        #region CreateImage
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TourImagesDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TourImagesDto dto)
        {
            if (ModelState.IsValid)
            {
                var imageUrl = _attachmentService.Upload(dto.ImageFile,"Images");
                var tourImage = new TourImage
                {
                    TourId = dto.TourId,
                    ImageUrl = imageUrl // Handle file upload separately
                };
                await _tourImageRepository.Add(tourImage);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        #endregion

    }
}
