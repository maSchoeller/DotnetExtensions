using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals
{
    internal static class StartupResolver
    {
        public static readonly string ConfigureApplicationMethodename = "ConfigureApplication";
        public static readonly string ConfigureServicesMethodename = "ConfigureServices";

        internal static object? CreateStartup(Type type, HostBuilderContext context)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            object? startup = null;

            if (!(type.GetConstructor(Type.EmptyTypes) is null))
            {
                startup = type.GetConstructor(Type.EmptyTypes)?.Invoke(Array.Empty<object>());
            }
            else if (!(type.GetConstructor(new Type[] { typeof(IConfiguration) }) is null))
            {
                startup = type.GetConstructor(new Type[] { typeof(IConfiguration) })?
                    .Invoke(new object[] { context.Configuration });
            }
            else if (!(type.GetConstructor(new Type[] { typeof(IHostEnvironment) }) is null))
            {
                startup = type.GetConstructor(new Type[] { typeof(IHostEnvironment) })?
                    .Invoke(new object[] { context.HostingEnvironment });
            }
            else if (!(type.GetConstructor(new Type[] { typeof(IConfiguration), typeof(IHostEnvironment) }) is null))
            {
                startup = type.GetConstructor(new Type[] { typeof(IConfiguration), typeof(IHostEnvironment) })?
                    .Invoke(new object[] { context.Configuration, context.HostingEnvironment });
            }
            else if (!(type.GetConstructor(new Type[] { typeof(IHostEnvironment), typeof(IConfiguration) }) is null))
            {
                startup = type.GetConstructor(new Type[] { typeof(IHostEnvironment), typeof(IConfiguration) })?
                    .Invoke(new object[] { context.HostingEnvironment, context.Configuration });
            }

            return startup;
        }

        internal static bool InvokeConfigureApplication(object startup, Application app, HostBuilderContext context)
        {
            //Todo: Maybe later cover IServiceProvider for injecting every service.

            var startuptype = startup.GetType();
            var methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(Application) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new[] { app });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(Application), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.Configuration });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(Application), typeof(IConfiguration), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.Configuration, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(Application), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(Application), typeof(IHostEnvironment), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.HostingEnvironment, context.Configuration });
                return true;
            }
            return false;
        }

        internal static bool InvokeConfigureServices(object startup, IServiceCollection services, HostBuilderContext context)
        {
            var startuptype = startup.GetType();
            var methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(IServiceCollection) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new[] { services });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(IServiceCollection), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { services, context.Configuration });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(IServiceCollection), typeof(IConfiguration), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { services, context.Configuration, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(IServiceCollection), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { services, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(ConfigureApplicationMethodename, new[] { typeof(IServiceCollection), typeof(IHostEnvironment), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { services, context.HostingEnvironment, context.Configuration });
                return true;
            }
            return false;
        }
    }
}
