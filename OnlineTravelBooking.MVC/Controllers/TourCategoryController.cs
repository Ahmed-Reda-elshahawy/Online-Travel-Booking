using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.MVC.Models;
using OnlineTravelBooking.Domain.Entities.Tour;
using System.Threading.Tasks;

namespace OnlineTravelBooking.MVC.Controllers
{
    public class TourCategoryController : Controller
    {
        private readonly IGenericRepository<TourCategory> _genericRepository;
        public TourCategoryController(IGenericRepository<TourCategory> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<IActionResult> Index()
        {
            var tourCategories =await _genericRepository.GetAll();
            List<TourCategoryDto> tourCategoryDtos = new List<TourCategoryDto>();
            foreach (var category in tourCategories)
            {
                tourCategoryDtos.Add(new TourCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }
            return View(tourCategoryDtos);
        }
        #region DeleteTourCategory
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            await _genericRepository.Delete(Id);
            return RedirectToAction("Index");
        }
        #endregion
        #region CreateTourCategory
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TourCategoryDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TourCategoryDto tourCategoryDto)
        {
            if (ModelState.IsValid)
            {
                TourCategory tourCategory = new TourCategory
                {
                    Name = tourCategoryDto.Name
                };
                await _genericRepository.Add(tourCategory);
                return RedirectToAction("Index");
            }
            return View(tourCategoryDto);
        }
        #endregion
        #region EditTourCategory
        [HttpGet]
        public async Task<IActionResult> Edit(int Id) { 
           var tourCategory= await _genericRepository.GetById(Id);
            var tourCategoryDto = new TourCategoryDto
            {
                Id = tourCategory.Id,
                Name = tourCategory.Name
            };
            return View(tourCategoryDto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TourCategoryDto tourCategory)
        {
            if (ModelState.IsValid)
            {
                await _genericRepository.Update(new TourCategory()
                {
                    Id = tourCategory.Id,
                    Name = tourCategory.Name
                });
                return RedirectToAction("Index");
            }
            return View(tourCategory);
        }
        #endregion

    }
}
