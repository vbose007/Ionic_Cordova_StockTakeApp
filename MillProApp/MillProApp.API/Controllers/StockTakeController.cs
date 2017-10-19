using MillProApp.API.Models;
using MillProApp.API.Services.Interfaces;
using System.Web.Http;
using System;
using MillProApp.API.Services.ServiceModels;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MillProApp.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/StockTake")]
    public class StockTakeController : BaseController
    {
        private readonly IStockTakeService _stockTakeService;

        public StockTakeController()
        {

        }

        public StockTakeController(IStockTakeService stockTakeService)
        {
            _stockTakeService = stockTakeService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllStockTakes()
        {
            var stockTakes = _stockTakeService.GetAllStockTakesForUser(User.Identity.GetUserId());
            return Json(stockTakes);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetStockTake(int id)
        {
            var stockTake = _stockTakeService.GetStockTake(id, User.Identity.GetUserId());

            if (stockTake == null)
                return NotFound();

            return Json(stockTake);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostStockTake([FromBody]StockTakeResource resource)
        {
            var svcModel = AutoMapper.Mapper.Map<StockTakeServiceModel>(resource);

            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(User.Identity.GetUserId());

            svcModel.CreatedByUserId = user.Id;

            resource = AutoMapper.Mapper.Map<StockTakeResource>(_stockTakeService.SaveStockTake(svcModel));

            return Created(GetResourceLocation(resource.Id), resource);
        }

        // DELETE: api/WorkOrders/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteStockTake(int id)
        {
             _stockTakeService.DeleteStockTake(id, User.Identity.GetUserId());

            return Ok();
        }

        private Uri GetResourceLocation(int resourceId)
        {
            var uri = Request.RequestUri.ToString();
            if (!uri.EndsWith("/"))
                uri += "/";

            return new Uri(String.Format(uri + resourceId));
        }
    }
}