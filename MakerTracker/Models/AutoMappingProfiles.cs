namespace MakerTracker.Models
{
    using System;
    using System.Linq;
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
            CreateMap<Need, NeedDto>(AutoMapper.MemberList.Destination)
                .ForMember(e => e.OutstandingQuantity, opts => opts.MapFrom(e => e.Transactions.Any() ? e.Quantity - e.Transactions.Sum(t => t.Amount) : e.Quantity));
            CreateMap<NeedDto, Need>(AutoMapper.MemberList.Source);
        }
    }
}