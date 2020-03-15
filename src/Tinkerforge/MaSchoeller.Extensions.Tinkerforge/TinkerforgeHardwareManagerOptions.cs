using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public class TinkerforgeHardwareManagerOptions
    {
        public int Port { get; set; } = 5001;

        public string Host { get; set; } = "Localhost";

        /// <summary>
        /// Sets the Delay for waiting while the underlying connection is established.
        /// </summary>
        public int StartupDelay { get; set; } = 1000;
    }
}
