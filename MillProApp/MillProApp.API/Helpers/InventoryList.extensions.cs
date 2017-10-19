using System.Collections.Generic;
using System.Linq;
using MillProApp.API.Models;

namespace MillProApp.API.Helpers
{
    public static class List 
    {
        public static bool EqualsInventoryList(this List<InventoryItemDto> inventoryList1, List<InventoryItemDto> inventorylist2)
        {
            if (inventoryList1.Count != inventorylist2.Count) return false;

            foreach (var item in inventoryList1)
            {
                bool contains = inventorylist2.Any(t => t.ItemCode == item.ItemCode);

                if (!contains) return false;
            }

            return true;
        }

    }
}