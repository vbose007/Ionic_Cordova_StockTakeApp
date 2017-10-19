using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MillProApp.Data.Models
{
    public class WorkOrder
    {
        [Key]
        public int Id { get; set; }
        public string WorkOrderNumber { get; set; }
        public string CreatedByUserId { get; set; }
        public int CreatedForCompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<InventoryItem> Inventory { get; set; }

        public WorkOrder()
        {
            Inventory = new List<InventoryItem>();
        }

    }
}