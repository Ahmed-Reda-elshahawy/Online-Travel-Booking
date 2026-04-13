using Microsoft.AspNetCore.Mvc;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.DTOs;
using OnlineTravelBooking.Domain.Entities.HotelEntity;
using OnlineTravelBooking.MVC.Models;
using OnlineTravelBooking.MVC.Models.HotelViewModel;


namespace OnlineTravelBooking.MVC.Controllers;

public class HotelController : Controller
{
	private readonly IHotelRepo _hotelRepo;
	private readonly IRoomRepo _roomRepo;

	public HotelController(IHotelRepo hotelRepo, IRoomRepo roomRepo)
	{
		_hotelRepo = hotelRepo;
		_roomRepo = roomRepo;
	}

	
	[HttpGet]
	public IActionResult Create()
	{
		return View(new HotelListVM
		{
			roomList = new List<RoomListVM>
		{
			new RoomListVM()
		}
		});
	}

	[HttpPost]
	public async Task<IActionResult> Create(HotelListVM model)
	{
		if (ModelState.IsValid)
		{
			Point? location = null;

			if (model.Latitude.HasValue && model.Longitude.HasValue)
			{
				var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
				location = geometryFactory.CreatePoint(
					new Coordinate(model.Longitude.Value, model.Latitude.Value)
				);
			}


			var hotel = new Hotel
			{
				Name = model.Name,
				Description = model.Description,
				Addresse = model.Address,
				City = model.City,
				Country = model.Country,
				PostalCode = model.PostalCode,
				Location = location,
				StarRating = model.StarRating,
				CheckInTime = model.CheckInTime,
				CheckOutTime = model.CheckOutTime,
				ContactPhone = model.ContactPhone,
				WebsiteURL = model.WebsiteURL
			};

			await _hotelRepo.AddHotelAsync(hotel);

			// Rooms
			if (model.roomList.Any())
			{
				var rooms = model.roomList.Select(r => new Room
				{
					HotelId = hotel.Id,
					RoomNum = r.RoomNum,
					BasePricePerNight = r.BasePricePerNight,
					Currency = r.Currency,
					Description = r.Description,
					Refundable = r.Refundable,
					MaxPeople = r.MaxPeople,
					Status = r.roomStatus,
					RoomType = r.roomType,
					availability = r.roomAvailability
				}).ToList();

				await _roomRepo.AddRoomsAsync(rooms);
			}

			return RedirectToAction(nameof(Index));
		}
		return View(model);
	}

	public async Task<IActionResult> Index(int pageNumber = 1)
	{
		var hotels = await _hotelRepo.GetAllHotelsAsync(pageNumber, 10);

		var model = hotels.Items.Select(h => new HotelListVM
		{
			Id = h.Id,
			Name = h.Name,
			City = h.City,
			Country = h.Country,
			StarRating = h.StarRating
		}).ToList();

		return View(model);
	}

	public async Task<IActionResult> Details(int id)
	{
		var hotel = await _hotelRepo.GetHotelByIdAsync(id);
		if (hotel == null) return NotFound();

		var rooms = await _roomRepo.GetAllRoomsByHotelIdAsync(id);

		var model = new HotelListVM
		{
			Id = hotel.Id,
			Name = hotel.Name,
			City = hotel.City,
			Country = hotel.Country,
			roomList = rooms.Select(r => new RoomListVM
			{
				RoomNum = r.RoomNum,
				BasePricePerNight = r.BasePricePerNight,
				MaxPeople = r.MaxPeople,
				roomStatus = r.Status,
				roomType = r.RoomType,
				roomAvailability = r.availability
			}).ToList()
		};

		return View(model);
	}

	//Update

	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		var hotel = await _hotelRepo.GetHotelByIdAsync(id);
		if (hotel == null) return NotFound();

		var model = new HotelListVM
		{
			Name = hotel.Name,
			Description = hotel.Description,
			Address = hotel.Addresse,
			City = hotel.City,
			Country = hotel.Country,
			PostalCode = hotel.PostalCode,
			StarRating = hotel.StarRating,
			CheckInTime = hotel.CheckInTime,
			CheckOutTime = hotel.CheckOutTime,
			ContactPhone = hotel.ContactPhone,
			WebsiteURL = hotel.WebsiteURL
		};

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, HotelListVM model)
	{
		if (!ModelState.IsValid)
			return View(model);

		var hotel = await _hotelRepo.GetHotelByIdAsync(id);
		if (hotel == null) return NotFound();
		hotel.Id = model.Id;
		hotel.Name = model.Name;
		hotel.Description = model.Description;
		hotel.Addresse = model.Address;
		hotel.City = model.City;
		hotel.Country = model.Country;
		hotel.PostalCode = model.PostalCode;
		hotel.StarRating = model.StarRating;
		hotel.CheckInTime = model.CheckInTime;
		hotel.CheckOutTime = model.CheckOutTime;
		hotel.ContactPhone = model.ContactPhone;
		hotel.WebsiteURL = model.WebsiteURL;

		await _hotelRepo.UpdateHotelAsync(hotel);

		return RedirectToAction(nameof(Index));
	}

	//Delete
	[HttpGet]
	public async Task<IActionResult> Delete(int id)
	{
		var hotel = await _hotelRepo.GetHotelByIdAsync(id);
		if (hotel == null) return NotFound();

		return View(hotel);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var hotel = await _hotelRepo.GetHotelByIdAsync(id);
		if (hotel == null) return NotFound();

		await _hotelRepo.DeleteHotelAsync(hotel);

		return RedirectToAction(nameof(Index));
	}


}
