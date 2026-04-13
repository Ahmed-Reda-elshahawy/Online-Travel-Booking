using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.TourRepos
{
    public class BookingTourRepo : IBookingTourRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BookingTourRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddBookingTourAsync(BookingTour bookingTour)
        {
            await _dbContext.Set<BookingTour>().AddAsync(bookingTour);
            _dbContext.SaveChanges();
        }


        public async Task<BookingTour> GetBookingTourDetailsAsync(int bookingTourId)
        {
           var res =  await _dbContext.Set<BookingTour>()
                .Include(bt => bt.TourSchedule)
                .FirstOrDefaultAsync(bt => bt.BookingId == bookingTourId);
            return res;
        }

        public void UpdateBookingTourAsync(BookingTour bookingTour)
        {
            _dbContext.Set<BookingTour>().Update(bookingTour);
            _dbContext.SaveChanges();
        }
        
    }
}
