using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillProApp.Data.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("WorkOrder")]
        public int WorkOrderId { get; set; }

        public string ItemCode { get; set; }

        public int Quantity { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

    }
}