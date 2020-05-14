using Autofac;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    public class Startup
    {
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<TestClass>();
            builder.RegisterType<ShellViewModel>();
        }

    }
}
