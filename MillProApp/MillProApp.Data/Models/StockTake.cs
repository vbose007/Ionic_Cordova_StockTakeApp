using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillProApp.Data.Models
{
    public class StockTake
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("User")]
        public string CreatedByUserId { get; set; }

        [ForeignKey("Company")]
        public int CreatedForCompanyId { get; set; }
        
        public List<InventoryItem> Inventory { get; set; }
        public Company Company { get; set; }
        public ApplicationUser User { get; set; }

        public StockTake()
        {
            Inventory = new List<InventoryItem>();
        }
    }
}