using MakerTracker.DBModels;
using MakerTracker.Models.Equipment;
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
            CreateMap<DBModels.Profile, ProfileDto>();
            CreateMap<UpdateProfileDto, DBModels.Profile>();
            CreateMap<DBModels.Equipment, EquipmentDto>(AutoMapper.MemberList.Destination);
            CreateMap<EquipmentDto, DBModels.Equipment>(AutoMapper.MemberList.Destination)
                .ForMember(e => e.UsedBy, opts => opts.Ignore());
        }
    }
}