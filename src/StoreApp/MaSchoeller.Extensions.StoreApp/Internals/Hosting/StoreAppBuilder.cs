using MaSchoeller.Extensions.StoreApp.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.StoreApp.Internals.Hosting
{
    internal class StoreAppBuilder<TShell> : StoreAppBuilder
        where TShell : Page, IStoreAppShell
    {
        public StoreAppBuilder(IHostBuilder builder) 
            : base(builder) { }

        public override void Build()
        {
            ConfigureServices(services => services.AddSingleton<IStoreAppShell, TShell>());
            base.Build();
        }
    }


    internal class StoreAppBuilder : IStoreAppBuilder
    {
        private Type _startupType;
        private Action<Application> _configureApplication;
        private Action<INavigationServiceBuilder> _configureNavigation;
        private Action<IServiceCollection> _configureServices;
        private readonly IHostBuilder _builder;

        public StoreAppBuilder(IHostBuilder builder)
        {
            _builder = builder;
        }

        public void ConfigureApplication(Action<Application> configure) 
            => _configureApplication += configure;

        public void ConfigureNavigation(Action<INavigationServiceBuilder> configure) 
            => _configureNavigation += configure;

        public void ConfigureServices(Action<IServiceCollection> configure) 
            => _configureServices += configure;

        public void UseStartup<Startup>()
        {
            _startupType = typeof(Startup);
        }

        public virtual void Build()
        {

        }
    }
}
