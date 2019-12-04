using MaSchoeller.Extensions.Universal.Abstracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureStoreApp(
            this IHostBuilder builder, 
            Action<IStoreAppBuilder> configure = null)
        {


            return builder;
        }

        public static IHostBuilder ConfigureStoreApp<TShell>(
            this IHostBuilder builder, 
            Action<IStoreAppBuilder> configure = null)
            where TShell : Page, IStoreAppShell
        {


            return builder;
        }
    }
}
