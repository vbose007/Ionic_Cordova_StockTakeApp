using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillProApp.API.Services.ServiceModels
{
    public class StockTakeServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string CreatedByUserId { get; set; }
        public int CreatedForCompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public List<InventoryItemServiceModel> Inventory { get; set; }
    }
}