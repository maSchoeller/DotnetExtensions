using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts.Sensors
{
    public interface IGpsSensor : ISensor<GpsData>
    {
    }

    public readonly struct GpsData
    {

    }
}
