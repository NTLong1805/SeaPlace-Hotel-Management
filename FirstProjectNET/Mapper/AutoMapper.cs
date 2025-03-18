using AutoMapper;
using FirstProjectNET.Models.ViewModel;
using FirstProjectNET.Models;
using static FirstProjectNET.Models.ViewModel.PropertiesViewModel;


namespace FirstProjectNET.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            // Main Page
            CreateMap<Services, HomeViewModel.ServiceViewModel>();
            CreateMap<Rate,HomeViewModel.RateViewModel>();
            CreateMap<Category,HomeViewModel.CategoryViewModel>();
            CreateMap<Category,PropertiesViewModel.CategoryViewModel>();


            CreateMap<Room, PropertiesViewModel.RoomAvailableViewModel>()
                .ForMember(dest => dest.RoomID , opt => opt.MapFrom(src => src.RoomID))
                .ForMember(dest => dest.ImageUrl , opt => opt.MapFrom(src => src.Images.Select(img => img.ImageUrl).ToList()))
                .ForMember(dest => dest.Status , opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Category , opt => opt.MapFrom(src => new PropertiesViewModel.CategoryViewModel
                {
                    CategoryID = src.Category.CategoryID,
                    TypeName = src.Category.TypeName,
                    Capacity = src.Category.Capacity,
                    Price = src.Category.Price
                }))
                .ReverseMap();

            CreateMap<Room, DetailRoomViewModel>()
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.Category.CategoryID))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Category.TypeName))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Category.Capacity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Category.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault().ImageUrl))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.RoomServices
                    .Select(rs => rs.Service.ServiceName).Distinct().ToList()));

            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Account.Username))
                .ForMember(dest => dest.AccountID, opt => opt.MapFrom(src => src.Account.AccountID.ToString()))
                .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => src.Membership));

            //Admin
            CreateMap<Booking,AdminBookingViewModel>().ReverseMap()
                .ForMember(dest => dest.DateCome, opt => opt.MapFrom(src => src.DateCome))
                .ForMember(dest => dest.DateGo, opt => opt.MapFrom(src => src.DateGo));

            CreateMap<Category, AdminCategoryViewModel>().ReverseMap();
        }

    }
}
