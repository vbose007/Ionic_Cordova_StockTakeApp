using AutoMapper;
using AutoMapper.Configuration;
using MillProApp.API.Models;
using MillProApp.API.Services.ServiceModels;
using MillProApp.Data.Models;

namespace MillProApp.API.Services.Helper
{
    public static class ServiceMapperConfig
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<StockTakeServiceModel, StockTakeResource>()
                    .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                    .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                    .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                    .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory));

                cfg.CreateMap<StockTakeResource, StockTakeServiceModel>()
                    .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                    .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));


                cfg.CreateMap<StockTake, StockTakeServiceModel>()
                    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                    .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                    .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                    .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

                cfg.CreateMap<StockTakeServiceModel, StockTake>()
                    .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                    .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));

                cfg.CreateMap<InventoryItem, InventoryItemServiceModel>()
                    .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

                cfg.CreateMap<InventoryItemServiceModel, InventoryItem>()
                    .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

                cfg.CreateMap<InventoryItemServiceModel, InventoryItemResource>()
                    .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

                cfg.CreateMap<InventoryItemResource, InventoryItemServiceModel>()
                    .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            });
        }
    }
}