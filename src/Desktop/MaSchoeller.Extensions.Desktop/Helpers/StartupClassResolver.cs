using Autofac;
using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Internals.Helpers
{
    public static class StartupClassResolver
    {
        public static readonly string ConfigureApplicationMethodename = "ConfigureApplication";
        public static readonly string ConfigureContainerMethodename = "ConfigureContainer";
        public static readonly string ConfigureServicesMethodename = "ConfigureServices";
        public static readonly string ConfigureNavigationMethodename = "ConfigureNavigation";

        internal static object? CreateStartup(
            Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            object? startup = null;

            if (!(type.GetConstructor(Type.EmptyTypes) is null))
            {
                startup = type.GetConstructor(Type.EmptyTypes)?
                    .Invoke(Array.Empty<object>());
            }
            //else if (!(type.GetConstructor(new Type[] { typeof(IConfiguration) }) is null))
            //{
            //    startup = type.GetConstructor(new Type[] { typeof(IConfiguration) })?
            //        .Invoke(new object[] { context.Configuration });
            //}
            //else if (!(type.GetConstructor(new Type[] { typeof(IHostEnvironment) }) is null))
            //{
            //    startup = type.GetConstructor(new Type[] { typeof(IHostEnvironment) })?
            //        .Invoke(new object[] { context.HostingEnvironment });
            //}
            //else if (!(type.GetConstructor(new Type[] { typeof(IConfiguration), typeof(IHostEnvironment) }) is null))
            //{
            //    startup = type.GetConstructor(new Type[] { typeof(IConfiguration), typeof(IHostEnvironment) })?
            //        .Invoke(new object[] { context.Configuration, context.HostingEnvironment });
            //}
            //else if (!(type.GetConstructor(new Type[] { typeof(IHostEnvironment), typeof(IConfiguration) }) is null))
            //{
            //    startup = type.GetConstructor(new Type[] { typeof(IHostEnvironment), typeof(IConfiguration) })?
            //        .Invoke(new object[] { context.HostingEnvironment, context.Configuration });
            //}

            return startup;
        }

        internal static bool InvokeConfigureContainer(object startup, ContainerBuilder container, HostBuilderContext context)
            => InnerInvokeConfigure(startup, ConfigureContainerMethodename, container, context);

        internal static bool InvokeConfigureApplication(object startup, Application app, HostBuilderContext context)
            => InnerInvokeConfigure(startup, ConfigureApplicationMethodename, app, context);
        internal static bool InvokeConfigureServices(object startup, IServiceCollection services, HostBuilderContext context)
            => InnerInvokeConfigure(startup, ConfigureServicesMethodename, services, context);
        internal static bool InvokeConfigureNavigation(object startup, INavigationServiceBuilder builder, HostBuilderContext context)
            => InnerInvokeConfigure(startup, ConfigureNavigationMethodename, builder, context);
        internal static bool InnerInvokeConfigure<TParam>(
            object startup,
            string methodename,
            TParam app,
            HostBuilderContext context)
            where TParam : class
        {
            return InnerInvokeConfigure(startup, methodename, typeof(TParam), app, context);
        }


        internal static bool InnerInvokeConfigure(
            object startup,
            string methodename,
            Type appType,
            object app,
            HostBuilderContext context)
        {
            var startuptype = startup.GetType();
            var methode = startuptype.GetMethod(methodename,
                new[] { appType });
            if (!(methode is null))
            {
                methode.Invoke(startup, new[] { app });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { appType, typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.Configuration });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { appType, typeof(IConfiguration), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.Configuration, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { appType, typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { appType, typeof(IHostEnvironment), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.HostingEnvironment, context.Configuration });
                return true;
            }
            return false;
        }
    }
}
