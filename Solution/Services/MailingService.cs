using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.UnitOfWork;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Services
{
    public class MailingService
    {
        private readonly IUnitOfWork _uow;
        private readonly string mail;
        private readonly string mailPassword;
        private readonly int port;
        private readonly string host;

        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public MailingService(IUnitOfWork uow)
        {
            _uow = uow;

            var sysConfigReadRepo = _uow.GetRepository<ISystemConfigReadRepository>();
            mail = sysConfigReadRepo.GetValue<string>("MailAddress");
            mailPassword = sysConfigReadRepo.GetValue<string>("MailPassword");
            port = sysConfigReadRepo.GetValue<int>("MailPort");
            host = sysConfigReadRepo.GetValue<string>("MailHost");
        }

        public void Send()
        {
            SendProd();
        }

        private void SendProd()
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(mail, "ZAMISR");
            var to = new EmailAddress(Receiver, "Receiver");
            var msg = MailHelper.CreateSingleEmail(from, to, Title, "", Body);
            var response = client.SendEmailAsync(msg).GetAwaiter().GetResult();
        }
    }
}
