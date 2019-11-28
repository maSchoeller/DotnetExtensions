using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Sample1
{
    public static class Program
    {
        public static async Task Main()
        {
            await Host.CreateDefaultBuilder()
                 .ConfigureWpfDefaults<ShellWindow>(b =>
                 {

                 })
                 .RunWpfAsync();
        }
    }
}
