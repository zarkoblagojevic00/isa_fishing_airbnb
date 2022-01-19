using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.UnitOfWork;
using Mailjet.Client;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

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
            if (Environment.GetEnvironmentVariable("IsOnServer") != "true")
            {
                SendDev();
            }
            else
            {
                SendProd();
            }
        }

        private void SendDev()
        {
            var client = ConfigureClient();
            var email = CreateMail();

            client.Send(email);
            client.Disconnect(true);
        }

        private void SendProd()
        {
            var client = new MailjetClient(Environment.GetEnvironmentVariable("ApiKey"),
                Environment.GetEnvironmentVariable("ApiSecret"));
            var request = new MailjetRequest()
            {
                Resource = Mailjet.Client.Resources.Send.Resource
            };
            request.Property(Mailjet.Client.Resources.Send.Messages, new JArray
            {
                new JObject {
                    {
                        "FromEmail",
                        "zamisr.isa@gmail.com"
                    }, {
                        "To",
                        Receiver
                    }, {
                        "Subject",
                        Title
                    }, {
                        "HTMLPart",
                        Body
                    }, {
                        "CustomID",
                        "Id"
                    }
                }
            });
            var response = client.PostAsync(request).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                SendDev();
            }
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
