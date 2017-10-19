using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using MillProApp.Data.Models;

namespace MillProApp.API.Helpers
{
    public class EmailHelper
    {
        /// <summary>
        /// This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">Recipient email address</param>
        /// <param name="attachmentFullPath"></param>
        /// <param name="bcc">Blind carbon copy email address</param>
        /// <param name="cc">Carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>

        #region Static Members
        public static void SendMailMessage(string toEmail, string fromEmail, string subject, string body, List<string> attachmentFullPath=null, string bcc=null, string cc=null )
        {
            //create the MailMessage object
            MailMessage mMailMessage = new MailMessage();

            if (!string.IsNullOrEmpty(fromEmail))
            {
                mMailMessage.From = new MailAddress(fromEmail);
            }

            mMailMessage.To.Add(new MailAddress(toEmail));

            if (!string.IsNullOrEmpty(bcc))
            {
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }

            if (!string.IsNullOrEmpty(cc))
            {
                mMailMessage.CC.Add(new MailAddress(cc));
            }

            if (string.IsNullOrEmpty(subject))
            {
                mMailMessage.Subject = "MillPro App Notification";
            }
            else
            {
                mMailMessage.Subject = subject;
            }

            mMailMessage.Body = body;

            mMailMessage.IsBodyHtml = false;

            mMailMessage.Priority = MailPriority.Normal;

            if (attachmentFullPath != null)
            {
                //add any attachments from the filesystem
                foreach (var attachmentPath in attachmentFullPath)
                {
                    Attachment mailAttachment = new Attachment(attachmentPath);
                    mMailMessage.Attachments.Add(mailAttachment);
                }
            }

            //create the SmtpClient instance
            SmtpClient mSmtpClient = new SmtpClient();

            //send the mail message
            mSmtpClient.Send(mMailMessage);
        }

        /// <summary>
        /// Determines whether an email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>
        /// 	<c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            // An empty or null string is not valid
            if (String.IsNullOrEmpty(emailAddress))
            {
                return (false);
            }

            // Regular expression to match valid email address
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            // Match the email address using a regular expression
            Regex re = new Regex(emailRegex);
            if (re.IsMatch(emailAddress))
                return (true);
            else
                return (false);
        }


        public static bool SendWorkOrderSummaryEmail(string toAddress, string workOrderNumber, string workOrderSummaryCsvFilePath)
        {
            string subject = "WorkOrder#" + workOrderNumber + " Summary";
            string body = "Please find attached inventory summary.";

            try
            {

                SendMailMessage(
                    toEmail: toAddress,
                    fromEmail: "vijay@phosphor.co.nz",
                    subject: subject,
                    body: body,
                    attachmentFullPath: new List<string>() {Path.GetFullPath(workOrderSummaryCsvFilePath)}
                );

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        #endregion
    }
}