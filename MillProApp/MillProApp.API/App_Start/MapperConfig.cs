using System;
using System.Globalization;
using AutoMapper;
using MillProApp.API.Models;
using MillProApp.API.Services.ServiceModels;
using MillProApp.Data.Models;

namespace MillProApp.API
{
    public class MapperConfig
    {
        public static void ConfigureMappings()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ApiMapProfile());
                cfg.AddProfile(new StockTakeMapProfile());
            });
        }
    }

    public class ApiMapProfile : Profile
    {

        public ApiMapProfile()
        {
            CreateMap<InventoryItem, InventoryItemDto>()
                .ForMember(dest => dest.Id, op => op.MapFrom(s => s.Id))
                .ForMember(dest => dest.ItemCode, op => op.MapFrom(s => s.ItemCode))
                .ForMember(dest => dest.Quantity, op => op.MapFrom(s => s.Quantity))
                .ForMember(dest => dest.WorkOrderId, op => op.MapFrom(s => s.WorkOrderId));

            CreateMap<InventoryItemDto, InventoryItem>()
            .ForMember(dest => dest.Id, op => op.MapFrom(s => s.Id))
            .ForMember(dest => dest.ItemCode, op => op.MapFrom(s => s.ItemCode))
            .ForMember(dest => dest.Quantity, op => op.MapFrom(s => s.Quantity))
            .ForMember(dest => dest.WorkOrderId, op => op.MapFrom(s => s.WorkOrderId))
            .ForMember(dest => dest.WorkOrder, op => op.Ignore());

            CreateMap<WorkOrder, WorkOrderDto>()
                .ForMember(dest => dest.Id, op => op.MapFrom(s => s.Id))
                .ForMember(dest => dest.CreatedByUserId, op => op.MapFrom(s => s.CreatedByUserId))
                .ForMember(dest => dest.CreatedDate, op => op.MapFrom(s => s.CreatedDate))
                .ForMember(dest => dest.CreatedForCompanyId, op => op.MapFrom(s => s.CreatedForCompanyId))
                .ForMember(dest => dest.CreatedByUserId, op => op.MapFrom(s => s.CreatedByUserId))
                .ForMember(dest => dest.WorkOrderNumber, op => op.MapFrom(s => s.WorkOrderNumber))
                .ForMember(dest => dest.Inventory, op => op.MapFrom(s => s.Inventory));

            CreateMap<WorkOrderDto, WorkOrder>()
                .ForMember(dest => dest.Id, op => op.MapFrom(s => s.Id))
                .ForMember(dest => dest.CreatedByUserId, op => op.MapFrom(s => s.CreatedByUserId))
                .ForMember(dest => dest.CreatedDate, op => op.MapFrom(s => s.CreatedDate))
                .ForMember(dest => dest.CreatedForCompanyId, op => op.MapFrom(s => s.CreatedForCompanyId))
                .ForMember(dest => dest.CreatedByUserId, op => op.MapFrom(s => s.CreatedByUserId))
                .ForMember(dest => dest.WorkOrderNumber, op => op.MapFrom(s => s.WorkOrderNumber))
                .ForMember(dest => dest.Inventory, op => op.MapFrom(s => s.Inventory))
            .ForMember(dest => dest.CreatedDate, op => op.MapFrom(s => !string.IsNullOrWhiteSpace(s.CreatedDate) ? DateTime.ParseExact(s.CreatedDate, "R", CultureInfo.InvariantCulture) : DateTime.UtcNow));
            
        }


    }


    public class StockTakeMapProfile : Profile
    {
        public StockTakeMapProfile()
        {
            CreateMap<StockTakeServiceModel, StockTakeResource>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory));

            CreateMap<StockTakeResource, StockTakeServiceModel>()
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));


            CreateMap<StockTake, StockTakeServiceModel>()
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

            CreateMap<StockTakeServiceModel, StockTake>()
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CreatedForCompanyId, opt => opt.MapFrom(src => src.CreatedForCompanyId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.Inventory))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));

            CreateMap<InventoryItem, InventoryItemServiceModel>()
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<InventoryItemServiceModel, InventoryItem>()
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<InventoryItemServiceModel, InventoryItemResource>()
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<InventoryItemResource, InventoryItemServiceModel>()
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));


        }
    }
}
