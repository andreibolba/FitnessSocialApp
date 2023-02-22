using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using API.Models;

namespace API.Utils
{
    public static class Utils
    {
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?.,><}{|@#$%^&*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static bool SendEmail(EmailFields details)
        {
            String SendMailFrom = "noreply.internhub@gmail.com";
            String SendMailTo = details.EmailTo;
            String SendMailSubject = details.EmailSubject;
            String SendMailBody = details.EmailBody;

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();

                email.From = new MailAddress(SendMailFrom);
                email.To.Add(SendMailTo);
                email.Subject = SendMailSubject;
                email.Body = SendMailBody;
                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "iofawdzjcufnnbso");
                SmtpServer.Send(email);
                return true;
            }
            catch (Exception ex)
            {
                switch(ex){
                    default:
                        return false;
                }
            }

        }

        public static bool UsernameExists(string username, InternShipAppSystemContext context)
        {
            return context.People.Any(person => person.Username == username);
        }

        public static bool EmailExists(string email, InternShipAppSystemContext context)
        {
            return context.People.Any(person => person.Email == email);
        }

    }
}