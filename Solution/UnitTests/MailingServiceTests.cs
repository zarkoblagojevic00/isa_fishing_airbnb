using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Domain.Entities;
using Domain.Mappings;
using Domain.UnitOfWork;
using FluentAssertions;
using Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace UnitTests
{
    [TestClass]
    [TestCategory("ServiceTests")]
    public class MailingServiceTests
    {
        private static IContainer container;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new NHibernateModule()
            {
                DbType = "SqlServer",
                ConnectionString = "Data Source=localhost;Initial Catalog=ISA;Integrated Security=True",
                MappingAssemblies = new List<Assembly>()
                {
                    typeof(AccountDeletionRequestMap).Assembly
                }
            });

            builder.RegisterModule(new RepositoryModule()
            {
                RepositoryAssemblies = new List<Assembly>()
                {
                    typeof(AccountDeletionRequest).Assembly
                }
            });

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            container = builder.Build();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            container.Dispose();
        }

        [TestMethod]
        public void SendTestEmail()
        {
            var uow = container.Resolve<IUnitOfWork>();

            uow.Should().NotBeNull();

            var mailingService = new MailingService(uow)
            {
                Receiver = "nikolamilosa20@gmail.com",
                Body = "Test",
                Title = "Test"
            };
            
            mailingService.Send();
        }
    }
}
