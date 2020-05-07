namespace MakerTracker.Models
{
    using MakerTracker.DBModels;
    using MakerTracker.Models.Products;
    using MakerTracker.Models.Profiles;
    using Profile = AutoMapper.Profile;

    public class AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<DBModels.Profile, ProfileDto>();
            CreateMap<UpdateProfileDto, DBModels.Profile>();
            CreateMap<Equipment, EquipmentDto>(AutoMapper.MemberList.Destination);
            CreateMap<EquipmentDto, Equipment>(AutoMapper.MemberList.Destination)
                .ForMember(e => e.UsedBy, opts => opts.Ignore());
            CreateMap<EquipmentDto, Equipment>(AutoMapper.MemberList.Destination)
                .ForMember(e => e.UsedBy, opts => opts.Ignore());
            CreateMap<MakerEquipment, MakerEquipmentDto>(AutoMapper.MemberList.Destination)
                .ForMember(e => e.EquipmentName, opts => opts.MapFrom(e => e.Equipment.Name));
            CreateMap<MakerEquipmentDto, MakerEquipment>(AutoMapper.MemberList.Source)
                .ForSourceMember(e => e.EquipmentName, opts => opts.DoNotValidate());
        }
    }
}