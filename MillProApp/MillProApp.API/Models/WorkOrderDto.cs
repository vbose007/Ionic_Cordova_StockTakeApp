using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using MillProApp.API.Helpers;
using MillProApp.Data.Models;

namespace MillProApp.API.Models
{

    public class WorkOrderDto //: IDataTransferObject<WorkOrder, WorkOrderDto>
    {
        public int Id { get; set; }
        public string WorkOrderNumber { get; set; }
        public string CreatedByUserId { get; set; }
        public int CreatedForCompanyId { get; set; }
        public string CreatedDate { get; set; }
        public List<InventoryItemDto> Inventory { get; set; }


        public WorkOrderDto()
        {
            Inventory = new List<InventoryItemDto>();
        }

        //public  WorkOrderDto FromEntity(WorkOrder workOrder)
        //{
        //    this.Id = workOrder.Id;
        //    this.WorkOrderNumber = workOrder.WorkOrderNumber;
        //    this.CreatedByUserId = workOrder.CreatedByUserId;
        //    this.CreatedForCompanyId = workOrder.CreatedForCompanyId;
        //    this.CreatedDate = workOrder.CreatedDate.ToString("R");
        //    var inventory = workOrder.Inventory ?? new List<InventoryItem>();
        //    this.Inventory = inventory.Select(x =>
        //    {
        //        var dto = new InventoryItemDto();
        //        return dto.FromEntity(x);
        //    }).ToList();
        //    return this;
        //}

        //public WorkOrder ToEntity()
        //{
        //    var workOrder = new WorkOrder();

        //    workOrder.Id = this.Id;
        //    workOrder.WorkOrderNumber = this.WorkOrderNumber;
        //    workOrder.CreatedByUserId = this.CreatedByUserId;
        //    workOrder.CreatedForCompanyId = this.CreatedForCompanyId;
        //    if (!string.IsNullOrWhiteSpace(this.CreatedDate))
        //    {
        //        workOrder.CreatedDate = DateTime.ParseExact(this.CreatedDate, "R", CultureInfo.InvariantCulture);
        //    }
        //    else
        //    {
        //        workOrder.CreatedDate = DateTime.UtcNow;
        //    }
        //    workOrder.Inventory = this.Inventory.Select(x => x.ToEntity()).ToList();
        //    return workOrder;
        //}

    }

}