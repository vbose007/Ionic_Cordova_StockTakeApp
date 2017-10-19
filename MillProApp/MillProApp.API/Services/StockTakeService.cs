using MillProApp.API.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using MillProApp.API.Services.ServiceModels;
using MillProApp.Data;
using MillProApp.Data.Models;

namespace MillProApp.API.Services
{
    public class StockTakeService : IStockTakeService
    {
        private readonly IStockTakeRepository _stockTakeRepository;

        public StockTakeService(IStockTakeRepository stockTakeRepository)
        {
            _stockTakeRepository = stockTakeRepository;
        }
        public StockTakeServiceModel SaveStockTake(StockTakeServiceModel stockTakeServiceModel)
        {
            var savedEntity =_stockTakeRepository.SaveStockTake(AutoMapper.Mapper.Map<StockTake>(stockTakeServiceModel));
            return AutoMapper.Mapper.Map<StockTakeServiceModel>(savedEntity);
        }

        public void DeleteStockTake(int stockTakeId, string userName)
        {
            _stockTakeRepository.DeleteStockTake(stockTakeId, userName);
        }

        public IList<StockTakeServiceModel> GetAllStockTakesForUser(string userId)
        {
            var stockTakes = _stockTakeRepository.GetStockTakesForUser(userId);

            return Mapper.Map<IList<StockTakeServiceModel>>(stockTakes);
        }

        public StockTakeServiceModel GetStockTake(int stockTakeId, string userName)
        {
            var entity = _stockTakeRepository.GetStockTake(stockTakeId, userName);
            return Mapper.Map<StockTakeServiceModel>(entity);
        }
    }
}