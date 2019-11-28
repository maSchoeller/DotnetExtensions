using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts
{
    public interface IDefaultWpfStartup
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureApplication(Application application);
    }
}
