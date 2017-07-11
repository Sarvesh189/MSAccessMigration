using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigration
{
   public class RegisterComponent
    {
        public static IContainer Container { get; set; }
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MSAccessAnalysis>().As<IMSAccessAnalysis>();
            builder.RegisterType<MSAccessTransfer>().As<IMSAccessTransfer>();
            builder.RegisterType<MigrationManager>().As<IMigrationManager>();
            Container = builder.Build();
             
        }

       
    }
}
