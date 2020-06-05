using System.Linq;
using AutoMapper;
using MakerTracker.DBModels;
using MakerTracker.Models.AdminReports;
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
            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<DBModels.Profile, ProfileDto>();
            CreateMap<DBModels.Profile, AdminProfileDto>();
            CreateMap<DBModels.Profile, SupplierReportDto>();
            CreateMap<DBModels.Profile, RequestorReportDto>();
            CreateMap<UpdateProfileDto, DBModels.Profile>();
            CreateMap<Equipment, EquipmentDto>(MemberList.Destination);
            CreateMap<EquipmentDto, Equipment>(MemberList.Destination)
                .ForMember(e => e.UsedBy, opts => opts.Ignore());
            CreateMap<EquipmentDto, Equipment>(MemberList.Destination)
                .ForMember(e => e.UsedBy, opts => opts.Ignore());
            CreateMap<MakerEquipment, MakerEquipmentDto>(MemberList.Destination)
                .ForMember(e => e.EquipmentName, opts => opts.MapFrom(e => e.Equipment.Name));
            CreateMap<MakerEquipmentDto, MakerEquipment>(MemberList.Source)
                .ForSourceMember(e => e.EquipmentName, opts => opts.DoNotValidate());
            CreateMap<Need, NeedDto>(MemberList.Destination)
                .ForMember(e => e.OutstandingQuantity,
                    opts => opts.MapFrom(e =>
                        e.Transactions.Any() ? e.Quantity - e.Transactions.Sum(t => t.Amount) : e.Quantity));
            CreateMap<NeedDto, Need>(MemberList.Source);
        }
    }
}