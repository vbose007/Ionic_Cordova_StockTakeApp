using MillProApp.Data.Models;
using System.Collections.Generic;

namespace MillProApp.Data
{
    public interface IStockTakeRepository
    {
        IList<StockTake> GetStockTakesForUser(string userId);

        StockTake SaveStockTake(StockTake stockTake);

        StockTake GetStockTake(int stockId, string userId);

        void DeleteStockTake(int stockId, string userId);
    }
}
