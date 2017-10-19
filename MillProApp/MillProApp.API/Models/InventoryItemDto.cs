using MillProApp.Data.Models;

namespace MillProApp.API.Models
{
    public class InventoryItemDto //:IDataTransferObject<InventoryItem, InventoryItemDto>
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }

        //public  InventoryItemDto FromEntity(InventoryItem inventoryItem)
        //{
        //    var inventoryItemDto  = new InventoryItemDto();

        //    inventoryItemDto.Id = inventoryItem.Id;
        //    inventoryItemDto.ItemCode = inventoryItem.ItemCode;
        //    inventoryItemDto.WorkOrderId = inventoryItem.WorkOrderId;
        //    inventoryItemDto.Quantity = inventoryItem.Quantity;
        //    return inventoryItemDto;
        //}

        //public InventoryItem ToEntity()
        //{
        //    var inventoryItem = new InventoryItem();
        //    inventoryItem.Id = this.Id;
        //    inventoryItem.WorkOrderId = this.WorkOrderId;
        //    inventoryItem.ItemCode = this.ItemCode;
        //    inventoryItem.Quantity = this.Quantity;

        //    return inventoryItem;
        //}

    }
}