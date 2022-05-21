using BookShop.Infrastructure;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;

namespace BookShop.Services
{
    public class EmailService
    {
        private string _adminPassword;
        public string AdminPassword
        {
            get => "";
            set
            {
                _adminPassword = value;
            }
        }
        public string AdminMail { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public SmtpClient Client { get; set; }

        public EmailService()
        {
            var configuration = new ConfigurationFactory().GetConfiguration();

            AdminMail = configuration["mailInfo:adminMail"];
            _adminPassword = configuration["mailInfo:adminPassword"];
            SmtpHost = configuration["mailInfo:smtpHost"];
            SmtpPort = Convert.ToInt16(configuration["mailInfo:smtpPort"]);

            //создаем подключение
            Client = new SmtpClient(SmtpHost, SmtpPort);
            Client.EnableSsl = true;
            Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            Client.UseDefaultCredentials = false;
            Client.Credentials = new NetworkCredential(AdminMail, _adminPassword);
        }

        public async Task SendMail(string to, string subject, string body, bool isBodyHtml = false)
        {
            //Создаем сообщение
            MailAddress From = new MailAddress(AdminMail, "BookShop");
            MailAddress To = new MailAddress(to);
            MailMessage mess = new MailMessage(From, To);
            mess.Subject = subject;
            mess.IsBodyHtml = isBodyHtml;
            mess.Body = body;

            try
            {
                await Client.SendMailAsync(mess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
    }
}
