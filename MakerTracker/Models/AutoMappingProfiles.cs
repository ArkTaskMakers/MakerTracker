using MakerTracker.DBModels;
using Profile = AutoMapper.Profile;

namespace MakerTracker.Models
{
    public class AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}