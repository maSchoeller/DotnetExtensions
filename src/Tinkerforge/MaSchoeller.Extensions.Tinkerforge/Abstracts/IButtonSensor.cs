using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IButtonSensor : ISensor<bool>
    {
        bool AsToggleButton { get; set; }
    }
}
