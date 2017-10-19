using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillProApp.API.Models
{
    public class InventoryItemResource
    {
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
    }
}