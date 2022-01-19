using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Domain.Entities;
using Domain.Mappings;
using Domain.UnitOfWork;
using Infrastructure;

namespace IntegrationTests.Base
{
    public class BaseFixture : IDisposable
    {
        private IContainer container { get; set; }
        public IUnitOfWork UoW { get; set; }
        public HttpClient Client { get; set; }
        public CookieContainer CookieContainer { get; set; }

        public BaseFixture()
        {
            SetupAutoFacDip();
            ResolveUnitOfWork();
            ConfigureHttpClient();
        }

        private void ConfigureHttpClient()
        {
            CookieContainer = new CookieContainer();
            var handler = new HttpClientHandler()
            {
                CookieContainer = CookieContainer
            };
            Client = new HttpClient(handler);
        }

        private void SetupAutoFacDip()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new NHibernateModule()
            {
                DbType = "SqlServer",
                ConnectionString = Environment.GetEnvironmentVariable("IsOnServer") != "true"
                    ? "Data Source=localhost;Initial Catalog=ISA;Integrated Security=True"
                    : "Server=localhost;Database=ISA;User Id=SA;Password=isaTEAM123@",
                MappingAssemblies = new List<Assembly>()
                {
                    typeof(AccountDeletionRequestMap).Assembly
                }
            });

            builder.RegisterModule(new RepositoryModule()
            {
                RepositoryAssemblies = new System.Collections.Generic.List<Assembly>()
                {
                    typeof(AccountDeletionRequest).Assembly
                }
            });

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            container = builder.Build();
        }

        private void ResolveUnitOfWork()
        {
            UoW = container.Resolve<IUnitOfWork>();
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
