using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyStarterApp.Services.Services
{
    // This will serve as the basic 'template' in the case an emailing sending service is desired for your particular application
    public class EmailTestService
    {
        public bool SendEmail()
        {
            bool sent = false;

            // Instantiating the client, using google's smtp server
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // Use a real gmail account below
            client.Credentials = new NetworkCredential("someCredibleEmail@mailinator.com", "somePass!");

            // The message to send
            MailMessage message = new MailMessage();
            // Another note of concern: Credentials' address overrides the 'From' information...while retaining the name
            // This is should be no problem assuming your company will be using just one email address to send emails, which should be the general case
            // Google requires the email address to be owned by YOU, so this is a set issue ^. There are work-arounds 
            // (May use other third-party email services instead, like SendGrid... I am too broke.)
            // In this case, I'm just using the below 'From' to set the name
            message.From = new MailAddress("someCredibleEmail@mailinator.com", "Kazeoseki");
            message.To.Add(new MailAddress("carrot@mailinator.com", "Carrot"));
            message.Subject = "TEST EMAIL! With SMTP...";
            // message.Body = "<h1>Some header</h1><p>Some paragraph..?</p><p>This should show on another line</p><i>And this should be italicized...<i>";

            // Attempting to use an external html file for the contents
            string newBody = HttpContext.Current.Server.MapPath("~/Email_Templates/email_confirmation.html");
            newBody = System.IO.File.ReadAllText(newBody);
            message.Body = newBody;
            message.IsBodyHtml = true;

            // Sending the message through the client
            client.Send(message);
            sent = true;

            // Other notes::
            // GMAIL has a limit on how many emails to send! Currently it is set to 100 emails per day.
            // Using GroupMail to send an email to multiple people in BULK however will save you from that limit.
            // My thoughts: For example, for confirmation email (warn: Will take within 24 hours to send...Unless using a business account)
            //     ...Send bulk confirmation emails at a certain time period (Hangfire?)
            return sent;
        }
    }
}
