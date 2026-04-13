using AutoMapper;
using OnlineTravelBooking.Application.DTOs.CarDtos;
using OnlineTravelBooking.Application.DTOs.BaseBookingDtos;
using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.Domain.Entities.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDto>();


            // DTO to Entity - Create
            CreateMap<CreateCarDto, Car>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.Bookings, opt => opt.Ignore());


            CreateMap<UpdateCarDto, Car>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

            // BaseBooking to BaseBookingDto mapping
            CreateMap<BaseBooking, BaseBookingDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName ?? string.Empty))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email ?? string.Empty))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));
        }
    }
}
