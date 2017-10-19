using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MillProApp.Data.Models;

namespace MillProApp.Data.Repositories
{
    public class StockTakeRepository : IStockTakeRepository
    {
        public IList<StockTake> GetStockTakesForUser(string userId)
        {

            using (var context = new ApplicationDbContext())
            {
                return context.StockTakes.Where(x => x.CreatedByUserId == userId).ToList();
            }
        }

        public StockTake SaveStockTake(StockTake stockTake)
        {

            using (var context = new ApplicationDbContext())
            {
                if (stockTake.Id != 0)
                {
                    var entity = context.StockTakes.FirstOrDefault(x => x.Id == stockTake.Id);
                    entity.Description = stockTake.Description;
                    entity.Location = stockTake.Location;
                    entity.Name = stockTake.Name;
                    context.InventoryItems.RemoveRange(entity.Inventory);
                    entity.Inventory = stockTake.Inventory;
                }
                else
                {
                    stockTake.CreatedDate = DateTime.Now;

                    context.StockTakes.Add(stockTake);
                }

                context.SaveChanges();

                return stockTake;
            }
        }

        public StockTake GetStockTake(int stockId, string userId)
        {

            using (var context = new ApplicationDbContext())
            {
                return context.StockTakes.FirstOrDefault(x => x.CreatedByUserId == userId && x.Id == stockId);
            }
        }

        public void DeleteStockTake(int stockId, string userId)
        {

            using (var context = new ApplicationDbContext())
            {
                var toDelete = context.StockTakes.FirstOrDefault(x => x.CreatedByUserId == userId && x.Id == stockId);
                context.StockTakes.Remove(toDelete);
                context.InventoryItems.RemoveRange(toDelete.Inventory);
                context.SaveChanges();
            }
        }
    }
}
