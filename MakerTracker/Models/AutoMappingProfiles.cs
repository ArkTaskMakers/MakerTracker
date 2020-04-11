using MakerTracker.DBModels;
using MakerTracker.Models.Products;
using MakerTracker.Models.Profiles;
using Profile = AutoMapper.Profile;

namespace MakerTracker.Models
{
    public class AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Profile, ProfileDto>();
        }
    }
}