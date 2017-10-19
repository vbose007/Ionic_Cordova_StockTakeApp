using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillProApp.API.Models
{
    public class StockTakeResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string CreatedByUserId { get; set; }
        public int CreatedForCompanyId { get; set; }
        public string CreatedDate { get; set; }
        public List<InventoryItemDto> Inventory { get; set; }
    }
}