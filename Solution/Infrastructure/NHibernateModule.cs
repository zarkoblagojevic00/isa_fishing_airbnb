using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using Environment = NHibernate.Cfg.Environment;
using Module = Autofac.Module;

namespace Infrastructure
{
    public class NHibernateModule : Module
    {
        public List<Type> MappingTypes { get; set; }
        public List<Assembly> MappingAssemblies { get; set; }
        public bool NamedRegister { get; set; } = false;
        public string Domain { get; set; }
        public string ConnectionString { get; set; }
        public string DbType { get; set; } = "SqlServer";

        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new Configuration()
                .SetProperty(Environment.ShowSql, "false")
                .SetProperty(Environment.FormatSql, "true")
                .SetProperty(Environment.ReleaseConnections, "on_close");

            if (DbType == "SqlServer")
            {
                configuration.AddProperties(new Dictionary<string, string>()
                {
                    { Environment.ConnectionDriver, typeof(Sql2008ClientDriver).FullName },
                    { Environment.Dialect, typeof(MsSql2008Dialect).FullName },
                    { Environment.ConnectionProvider, typeof(DriverConnectionProvider).FullName },
                    { Environment.ConnectionString, ConnectionString }
                });
            }
            else if (DbType == "PostgreSql")
            {
                //TODO: PostgreSql
            }

            var mapper = new ModelMapper();

            MappingTypes?.ForEach(type =>
            {
                mapper.AddMapping(type);
            });

            MappingAssemblies?.ForEach(assembly =>
            {
                mapper.AddMappings(assembly.GetTypes());
            });

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            builder.RegisterInstance(configuration).As<Configuration>().SingleInstance();

            if (NamedRegister)
            {
                builder.Register(c => configuration.BuildSessionFactory()).Named<ISessionFactory>(Domain).SingleInstance();
                builder.Register(c => c.ResolveNamed<ISessionFactory>(Domain).OpenSession()).Named<ISession>(Domain);
            }
            else
            {
                builder.Register(c => configuration.BuildSessionFactory()).As<ISessionFactory>().SingleInstance();
                builder.Register(c => c.Resolve<ISessionFactory>().OpenSession());
            }
        }
    }
}
