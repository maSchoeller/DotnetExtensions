using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IJoyStickSensor : ISensor<(double X, double Y)>
    {

    }
}
