using Autofac;

using MaSchoeller.Extensions.Desktop.Abstracts;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
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


        internal static bool TryInvokeConfigureContainer(
            object startup, ContainerBuilder container, HostBuilderContext context)
        {
            var list = CreateNewContextDependencies(context);
            list.Add((container.GetType(), container));
            return TryInvokeMethode(startup, ConfigureContainerMethodename, list);
        }

        internal static bool TryInvokeConfigureServices(
            object startup, IServiceCollection container, HostBuilderContext context)
        {
            var list = CreateNewContextDependencies(context);
            list.Add((typeof(IServiceCollection), container));
            return TryInvokeMethode(startup, ConfigureServicesMethodename, list);
        }

        internal static bool TryInvokeConfigureNavigationMvvm(
            object startup, INavigationServiceBuilder builder, HostBuilderContext context)
        {
            var list = CreateNewContextDependencies(context);
            list.Add((typeof(INavigationServiceBuilder), builder));
            return TryInvokeMethode(startup, ConfigureNavigationMethodename, list);
        }

        internal static bool TryInvokeConfigureApplication(
           object startup, Application app, HostBuilderContext context)
        {
            var list = CreateNewContextDependencies(context);
            list.Add((app.GetType(), app));
            return TryInvokeMethode(startup, ConfigureApplicationMethodename, list);
        }

        internal static bool TryInvokeMethode(object startupInstance,
        string methodname, IEnumerable<(Type type, object instance)> dependencies)
        {
            var collection = new ServiceCollection();
            foreach (var (type, instance) in dependencies)
                collection.AddSingleton(type, type);
            var provider = collection.BuildServiceProvider();


            var methodDefinition = startupInstance
                .GetType()
                .GetMethods()
                .FirstOrDefault(m => m.Name == methodname);
            if (methodDefinition is null)
                return false;
            var parameter = methodDefinition
                .GetParameters()
                .Select(t => provider.GetService(t.ParameterType))
                .ToArray();
            methodDefinition.Invoke(startupInstance, parameter);
            return true;
        }

        private static ICollection<(Type type, object instance)> CreateNewContextDependencies(HostBuilderContext context)
        {
            var list = new List<(Type, object)>
            {
                (context.GetType(), context),
                (typeof(IHostEnvironment), context.HostingEnvironment),
                (typeof(IConfiguration), context.Configuration),
                (typeof(IDictionary<object, object>), context.Properties)
            };
            return list;
        }
    }
}
