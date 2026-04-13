using Microsoft.AspNetCore.Mvc;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Tour;
using OnlineTravelBooking.MVC.Models;
using System.Threading.Tasks;

namespace OnlineTravelBooking.MVC.Controllers
{
    public class TourScheduleController : Controller
    {
        private readonly IGenericRepository<TourSchedule> _tourScheduleRepository;
        public TourScheduleController(IGenericRepository<TourSchedule> tourScheduleRepository)
        {
            _tourScheduleRepository = tourScheduleRepository;
        }
        public async Task<IActionResult> Index()
        {
           var tourSchdules =await _tourScheduleRepository.GetAll();
           List<TourScheduleDto> tourScheduleDtos = new List<TourScheduleDto>();
              foreach (var schedule in tourSchdules)
            {
               tourScheduleDtos.Add(new TourScheduleDto
               {
                   Id = schedule.Id,
                   TourId = schedule.TourId,
                   StartDate = schedule.StartDate,
                   EndDate = schedule.EndDate,
                   AvailableSeats = schedule.AvailableSeats
               });
            }
                return View(tourScheduleDtos);
        }
        #region DeleteSchedule
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var schedule = await _tourScheduleRepository.GetById(Id);
            if (schedule == null)
            {
                return NotFound();
            }
            await _tourScheduleRepository.Delete(Id);
            return RedirectToAction("Index");
        }
        #endregion
        #region CreateSchedule
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TourScheduleDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(TourScheduleDto dto)
        {
            if (ModelState.IsValid)
            {
                var schedule = new TourSchedule
                {
                    TourId = dto.TourId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    AvailableSeats = dto.AvailableSeats
                };
                await _tourScheduleRepository.Add(schedule);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        #endregion
        #region EditSchedule
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var schedule = await _tourScheduleRepository.GetById(Id);
            if (schedule == null)
            {
                return NotFound();
            }
            var dto = new TourScheduleDto
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                AvailableSeats = schedule.AvailableSeats
            };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TourScheduleDto dto)
        {
            if (ModelState.IsValid)
            {
                var schedule = await _tourScheduleRepository.GetById(dto.Id);
                if (schedule == null)
                {
                    return NotFound();
                }
                schedule.Id = dto.Id;
                schedule.TourId = dto.TourId;
                schedule.StartDate = dto.StartDate;
                schedule.EndDate = dto.EndDate;
                schedule.AvailableSeats = dto.AvailableSeats;
                await _tourScheduleRepository.Update(schedule);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        #endregion
    }
}
