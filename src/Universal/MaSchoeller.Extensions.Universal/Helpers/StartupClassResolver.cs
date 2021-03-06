﻿using MaSchoeller.Extensions.Universal.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace MaSchoeller.Extensions.Universal.Helpers
{
    public static class StartupClassResolver
    {
        public static readonly string ConfigureServicesMethodename = "ConfigureServices";
        public static readonly string ConfigureNavigationMethodename = "ConfigureNavigation";

        internal static object CreateStartup(
            Type type,
            HostBuilderContext context)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            object startup = null;

            if (!(type.GetConstructor(Type.EmptyTypes) is null))
            {
                startup = type.GetConstructor(Type.EmptyTypes)?
                    .Invoke(Array.Empty<object>());
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
            //Todo: Maybe later cover IServiceProvider for injecting every service.

            var startuptype = startup.GetType();
            var methode = startuptype.GetMethod(methodename,
                new[] { typeof(TParam) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new[] { app });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { typeof(TParam), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.Configuration });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { typeof(TParam), typeof(IConfiguration), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.Configuration, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { typeof(TParam), typeof(IHostEnvironment) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.HostingEnvironment });
                return true;
            }
            methode = startuptype.GetMethod(methodename,
                new[] { typeof(TParam), typeof(IHostEnvironment), typeof(IConfiguration) });
            if (!(methode is null))
            {
                methode.Invoke(startup, new object[] { app, context.HostingEnvironment, context.Configuration });
                return true;
            }
            return false;
        }
    }
}
