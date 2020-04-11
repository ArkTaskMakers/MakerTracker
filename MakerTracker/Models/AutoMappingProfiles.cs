using MakerTracker.DBModels;
using MakerTracker.Models.Products;
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