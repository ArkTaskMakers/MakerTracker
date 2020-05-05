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
            CreateMap<MakerStock, MakerStockDto>(AutoMapper.MemberList.Destination)
                .ForMember(e => e.ProductName, opts => opts.MapFrom(e => e.Product.Name));
            CreateMap<MakerStockDto, MakerStock>(AutoMapper.MemberList.Source)
                .ForSourceMember(e => e.ProductName, opts => opts.DoNotValidate());
        }
    }
}