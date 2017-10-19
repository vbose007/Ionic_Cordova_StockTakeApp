using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MillProApp.API.Models;
using MillProApp.Data.Models;

namespace MillProApp.API.Helpers
{
    public class InventoryReportHelper
    {
        private readonly static string path = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\WorkOrderSummary"; //@"C:" + $@"\App_Data\WorkOrderSummary";
        private static string _toEmailAddress = "viju.bose@gmail.com";


        public static void GenerateAndSendInventoryReportForWorkOrder(WorkOrderDto workOrder, string toEmail)
        {
            if(string.IsNullOrWhiteSpace(workOrder?.WorkOrderNumber)) return;

            var workOrderSummaryCsv = new StringBuilder();//CsvGenerator.ToCsv(",", workOrder.Inventory);

            workOrderSummaryCsv.AppendLine("WorkOrder#," + workOrder.WorkOrderNumber);
            workOrderSummaryCsv.AppendLine("");
            workOrderSummaryCsv.Append(CsvGenerator.ToCsv(",", workOrder.Inventory));

            string workOrderNumber = workOrder.WorkOrderNumber;

            var filePath = path + $@"\{workOrderNumber}.csv";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            CreateCSVFile(workOrderNumber, workOrderSummaryCsv.ToString(), filePath);

            toEmail = string.IsNullOrWhiteSpace(toEmail) ? _toEmailAddress : toEmail;
           
            EmailHelper.SendWorkOrderSummaryEmail(toEmail, workOrder.WorkOrderNumber, filePath);


        }

        private static void CreateCSVFile(string workOrderNumber, string csv, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.AppendAllText(filePath, csv);
            }
            catch (Exception e)
            {
                
            }

        }
    }
}