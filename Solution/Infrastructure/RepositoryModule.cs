using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Module = Autofac.Module;

namespace Infrastructure
{
    public class RepositoryModule : Module
    {
        public List<Assembly> RepositoryAssemblies { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(RepositoryAssemblies.ToArray())
                .Where(t => t.Namespace != null && t.Namespace.Contains("Domain.Repositories"))
                .AsImplementedInterfaces();
        }
    }
}
