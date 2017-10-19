using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Routing;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MillProApp.API.Helpers;
using MillProApp.API.Models;
using MillProApp.Data.Models;
using MillProApp.Data.Repositories;
using Newtonsoft.Json.Linq;
using MillProApp.Data;

namespace MillProApp.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/WorkOrders")]
    //[EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class WorkOrdersController : ApiController
    {
        //private static MockDatabase db = new MockDatabase(); 
        private WorkOrderRepository workOrderRepo = new WorkOrderRepository();

        public IMapper Map
        {
            get { return Mapper.Configuration.CreateMapper(); }
        }

        // GET: api/WorkOrders
        [Route("")]
        public List<WorkOrderDto> GetWorkOrders()
        {
            return workOrderRepo.GetAllWorkOrders().ToList().Select(x => Mapper.Map<WorkOrderDto>(x)).ToList();
            //return workOrderRepo.GetAllWorkOrders().ToList().Select(x => new WorkOrderDto().FromEntity(x)).ToList();
        }



        // GET: api/WorkOrders/5
        [Route("{id:int}")]
        [ResponseType(typeof(WorkOrder))]
        public IHttpActionResult GetWorkOrder(int id)
        {
            WorkOrder workOrder = workOrderRepo.GetWorkOrder(id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return Ok(workOrder);
        }

        // GET: api/WorkOrders/ASDF1234
        [ResponseType(typeof(WorkOrderDto))]
        [Route("{workOrderNumber}")]
        public IHttpActionResult GetWorkOrder(string workOrderNumber)
        {
            try
            {
                var userCompany = GetCurrentUserCompany();

                IList<WorkOrder> workOrders = workOrderRepo.GetWorkOrdersByWorkOrderNumber(workOrderNumber);
                
                if (workOrders == null)
                {
                    return NotFound();
                }
                // Filter workOrders to only return workorders for the requesting company
                workOrders = workOrders.Where(w => w.CreatedForCompanyId == userCompany.Id).ToList();

                return Ok(workOrders.Select(x => Mapper.Map<WorkOrderDto>(x)).ToList());
                //return Ok(workOrders.Select(x => new WorkOrderDto().FromEntity(x)).ToList());
            }
            catch (HttpResponseException e)
            {
                return ResponseMessage(Request.CreateErrorResponse(e.Response.StatusCode, e.Message));
            }
        }

        private Company GetCurrentUserCompany()
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = userManager.FindById(User.Identity.GetUserId());

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    //Exception(httpCode: 401, message: "Access denied. Invalid user credentials.");

            using (var context = new ApplicationDbContext())
            {
                if (user.CompanyId > 0)
                {
                    var company = context.Companys.FirstOrDefault(c => c.Id == user.CompanyId);

                    return company;
                }
            }

            return null;


        }
    

        // POST: api/WorkOrders
        [ResponseType(typeof(WorkOrderDto))]
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostWorkOrder([FromBody]WorkOrderCompletionRequest workOrderCompletionRequest)
        {
            if (workOrderCompletionRequest == null)
            {
                return BadRequest(ModelState);
            }

            var workOrderDto = workOrderCompletionRequest.WorkOrder;
            //var toEmail = workOrderCompletionRequest.toEmail;

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(workOrderDto?.WorkOrderNumber))
            {
                return BadRequest(ModelState);
            }

            var userCompany = GetCurrentUserCompany();

            if (userCompany != null)
            {
                workOrderDto.CreatedForCompanyId = userCompany.Id;
            }

            var workOrder = Mapper.Map<WorkOrder>(workOrderDto);
            //var workOrder = workOrderDto.ToEntity();


            var workOrderFromDb = workOrderRepo.TryGetMatchingWorkOrderFromDb(workOrder);


            if (workOrderFromDb != null && workOrderFromDb.Id > 0 ) //If work order already exists
            {
                UpdateWorkOrder(workOrderFromDb, workOrderDto);
            }
            else
            {
                workOrderDto = AddNewWorkOrder(workOrder);
            }

            InventoryReportHelper.GenerateAndSendInventoryReportForWorkOrder(workOrderDto, userCompany.Email);
            //InventoryReportHelper.GenerateAndSendInventoryReportForWorkOrder(workOrderDto, toEmail);

            return Json(workOrderDto);
            //return CreatedAtRoute("DefaultApi", new { id = workOrder.Id }, workOrder);
        }

        private WorkOrderDto AddNewWorkOrder(WorkOrder workOrder )
        {
            workOrder.CreatedByUserId = RequestContext.Principal.Identity.GetUserId();
            workOrder.CreatedDate = DateTime.UtcNow;
            var workOrderEntity = workOrderRepo.CreateWorkOrder(workOrder);
            
            var workOrderDto = Mapper.Map<WorkOrderDto>(workOrderEntity);
            //workOrderDto = workOrderDto.FromEntity(workOrderEntity);
            return workOrderDto;
        }

        private void UpdateWorkOrder(WorkOrder workOrderFromDb, WorkOrderDto workOrderDto)
        {
            var workOrderDtoFromDb = Mapper.Map<WorkOrderDto>(workOrderFromDb);
            //var workOrderDtoFromDb = new WorkOrderDto().FromEntity(workOrderFromDb);

            //if (workOrderDtoFromDb.Equals(workOrderDto))
            if (workOrderDtoFromDb.WorkOrderNumber.Equals(workOrderDto.WorkOrderNumber, StringComparison.OrdinalIgnoreCase)
                && workOrderDtoFromDb.CreatedForCompanyId == workOrderDto.CreatedForCompanyId)
            {
                workOrderRepo.UpdateWorkOrder(Mapper.Map<WorkOrder>(workOrderDto));
                //workOrderRepo.UpdateWorkOrder(workOrderDto.ToEntity());
            }
        }



        // DELETE: api/WorkOrders/5
        [ResponseType(typeof(int))]
        public IHttpActionResult DeleteWorkOrder(int id)
        {
            var workOrder = workOrderRepo.DeleteWorkOrder(id);

            if (workOrder == null)
            {
                return NotFound();
            }
            return Ok(id);
        }

    }

}