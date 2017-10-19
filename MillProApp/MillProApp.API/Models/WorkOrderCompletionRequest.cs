using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillProApp.API.Models
{
    public class WorkOrderCompletionRequest
    {
        public WorkOrderDto WorkOrder { get; set; }
        //public string toEmail { get; set; }
    }
}