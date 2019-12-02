using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Extensions.Desktop.Internals.Hosting
{
    internal static class ApplicationBuilder
    {
        public static async Task<Application> CreateIfNotExistsAsync(ShutdownMode shutdownMode)
        {
            if (Application.Current is null)
            {
                await Task.Run(() =>
                {
                    var uiThread = new Thread(() =>
                    {
                        SynchronizationContext.SetSynchronizationContext(
                            new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                        new Application
                        {
                            ShutdownMode = shutdownMode
                        }
                        .Run(); //Thread is Blocking
                });
                    uiThread.IsBackground = true;
                    uiThread.SetApartmentState(ApartmentState.STA);
                    uiThread.Start();
                });
                await Task.Delay(400); //Wait a small time to get application ready.
            }
            return Application.Current!;
        }
    }
}
