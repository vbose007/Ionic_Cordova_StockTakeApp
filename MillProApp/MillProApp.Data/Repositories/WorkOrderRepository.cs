using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MillProApp.Data.Models;

namespace MillProApp.Data.Repositories
{
    public class WorkOrderRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<WorkOrder> GetAllWorkOrders()
        {
            return db.WorkOrders.AsQueryable<WorkOrder>();
        }

        public WorkOrder GetWorkOrder(int id)
        {
            return db.WorkOrders.Find(id);
        }

        public WorkOrder DeleteWorkOrder(int id)
        {
            WorkOrder workOrder = db.WorkOrders.Find(id);
            if (workOrder == null)
            {
                return null;
            }

            db.WorkOrders.Remove(workOrder);
            db.SaveChanges();
            return workOrder;
        }

        public void UpdateWorkOrder(WorkOrder workOrder)
        {
            if (string.IsNullOrEmpty(workOrder?.WorkOrderNumber)) return;

            var workOrderInDb = TryGetMatchingWorkOrderFromDb(workOrder);

            if (workOrderInDb == null)
            {
                //workOrderInDb = GetWorkOrderByWorkOrderNumber(workOrder.WorkOrderNumber);

                //if(workOrderInDb == null)
                return;
            }
            else //update record
            {
                workOrderInDb.Inventory = workOrder.Inventory;
            }

            db.Entry(workOrderInDb).State = EntityState.Modified;
            db.SaveChanges();
        }

        public WorkOrder CreateWorkOrder(WorkOrder workOrder)
        {
            db.WorkOrders.Add(workOrder);
            db.SaveChanges();
            //return GetWorkOrderByWorkOrderNumber(workOrder.WorkOrderNumber);
            return TryGetMatchingWorkOrderFromDb(workOrder);
        }

        public bool WorkOrderExists(int id)
        {
            return db.WorkOrders.Any(e => e.Id == id);
        }

        public bool WorkOrderExists(string workOrderNumber)
        {
            return db.WorkOrders.Any(e => e.WorkOrderNumber == workOrderNumber);
        }

        //public WorkOrder GetWorkOrderByWorkOrderNumber(string workOrderNumber)
        //{
        //    return db.WorkOrders.FirstOrDefault(w => w.WorkOrderNumber == workOrderNumber);
        //}

        public IList<WorkOrder> GetWorkOrdersByWorkOrderNumber(string workOrderNumber)
        {
            return db.WorkOrders.Where(w => w.WorkOrderNumber == workOrderNumber).ToList();
        }

        public IList<WorkOrder> GetWorkOrdersByWorkOrderNumberAndCompanyId(string workOrderNumber, int companyId)
        {
            return db.WorkOrders.Where(w => w.WorkOrderNumber == workOrderNumber && w.CreatedForCompanyId == companyId).ToList();
        }
        public WorkOrder TryGetMatchingWorkOrderFromDb(WorkOrder workOrder)
        {
            var workOrderFromDb = GetWorkOrdersByWorkOrderNumberAndCompanyId(workOrder.WorkOrderNumber, workOrder.CreatedForCompanyId)?.FirstOrDefault();

            //var workOrderFromDb = GetWorkOrdersByWorkOrderNumber(workOrder.WorkOrderNumber)
            //    ?.FirstOrDefault(x => x.Equals(workOrder));
            return workOrderFromDb;
        }
    }
}