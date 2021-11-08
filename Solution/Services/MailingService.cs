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
            var client = ConfigureClient();
            var email = CreateMail();

            client.Send(email);
            client.Disconnect(true);
        }

        private SmtpClient ConfigureClient()
        {
            var smtpClient = new SmtpClient();

            smtpClient.Connect(host, port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(mail, mailPassword);

            return smtpClient;
        }

        private MimeMessage CreateMail()
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(mail));
            email.To.Add(MailboxAddress.Parse(Receiver));

            email.Subject = Title;
            email.Body = new TextPart(TextFormat.Html) { Text = Body };

            return email;
        }
    }
}
