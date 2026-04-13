using OnlineTravelBooking.Application.DTOs.TourDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Interfaces.Services
{
    public interface ITourCategoryService
    {
        Task<int> AddTourCategory(TourCategoryDto tourCategoryDto);
        Task<int> DeleteTourCategory(int id);
        Task<int> UpdateTourCategory(TourCategoryDto tourCategoryDto);
        Task<IEnumerable<TourCategoryDto>> GetTourCategories();
        Task<TourCategoryDto> GetTourCategoryById(int id);
    }
}
