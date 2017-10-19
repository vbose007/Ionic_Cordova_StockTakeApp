using MillProApp.API.Models;
using MillProApp.API.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MillProApp.API.Services.Interfaces
{
    public interface IStockTakeService
    {
        IList<StockTakeServiceModel> GetAllStockTakesForUser(string userId);

        StockTakeServiceModel GetStockTake(int stockTakeId, string userId);

        void DeleteStockTake(int stockTakeId, string userId);

        StockTakeServiceModel SaveStockTake(StockTakeServiceModel stockTakeServiceModel);
    }
}
