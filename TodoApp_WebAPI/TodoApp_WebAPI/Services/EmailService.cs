using System.Net;
using System.Net.Mail;
using TodoApp_WebAPI.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace TodoApp_WebAPI.Services
{
    public class EmailService
    {
        private static IConfiguration AppSetting { get; set; }
        private string senderEmail;
        private string password;
        public EmailService()
        {
            AppSetting = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            senderEmail = AppSetting["MailAccount:Email"];
            password = AppSetting["MailAccount:Password"];
        }

        public void SendEmail(string receiverEmail, Task task)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                smtpClient.EnableSsl = true;
                MailMessage mailMessage = new MailMessage();
                mailMessage = DuedateMail(senderEmail, receiverEmail, task);

                smtpClient.Send(mailMessage);
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }

        private MailMessage DuedateMail(string emailFrom, string emailTo, Task task)
        {
            MailMessage duedateMessage = new MailMessage();
            try
            {
                duedateMessage.From = new MailAddress(emailFrom);
                duedateMessage.Subject = "Hey, You have upcoming task";
                duedateMessage.IsBodyHtml = true;
                string taskName = task.Name;
                string taskDuedate = ((DateTime)task.DueDate).ToString("MM/dd/yyyy hh:mm tt");
                //duedateMessage.Body = "<h1>Hey there</h1><br/>Your task: <b>"+taskName+"</b> was due at "+taskDuedate;
                duedateMessage.Body =
                    "<head><link href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u' crossorigin='anonymous'></head><div class='card'><div class='card-header'>To Do App</div>" +
                    "<div class='card-body'>"+
                        "<h5 class='card-title'>Your task was due!</h5>"+
                        "<p class='card-text'>Hello, your task <b>"+taskName+"</b> was due at " +taskDuedate +".</p>"+
                    "</div>"+
                    "</div>";
                  duedateMessage.To.Add(emailTo);
            }
            catch (Exception)
            {

                throw;
            }
            return duedateMessage;
        }




    }
}
