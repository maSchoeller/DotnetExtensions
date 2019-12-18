using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts.Actors
{
    public interface IMotorManager : IMultiActor<byte,IMotor>
    {
    }
}
