using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Internals.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureUniversal(
            this IHostBuilder builder,Type mainpage, Type shellViewModel, 
            Action<IUniversalBuilder> configure = null,
            bool enableNavigation = true)
        {
            var ubuilder = new UniversalBuilder(builder, enableNavigation,mainpage,shellViewModel);
            configure?.Invoke(ubuilder);
            ubuilder.Build();
            return builder;
        }

        public static IHostBuilder ConfigureUniversal<TShell,TShellViewModel>(
            this IHostBuilder builder, 
            Action<IUniversalBuilder> configure = null, 
            bool enableNavigation = true)
            where TShell : Page
        {
            var ubuilder = new UniversalBuilder<TShell,TShellViewModel>(builder, enableNavigation);
            configure?.Invoke(ubuilder);
            ubuilder.Build();
            return builder;
        }

        public static IHostBuilder AddUniversalDefaults(this IHostBuilder builder, params string[] args)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables(prefix: "Universal_");
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                if (env.IsDevelopment() && !string.IsNullOrEmpty(env.ApplicationName))
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true);
                    }
                }

                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                //var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                //// IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                //// the defaults be overridden by the configuration.
                //if (isWindows)
                //{
                //    // Default the EventLogLoggerProvider to warning or above
                //    logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Warning);
                //}

                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();

                //if (isWindows)
                //{
                //    // Add the EventLogLoggerProvider on windows machines
                //    logging.AddEventLog();
                //}
            })
            .UseDefaultServiceProvider((context, options) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            });

            return builder;
        }

    }
}
