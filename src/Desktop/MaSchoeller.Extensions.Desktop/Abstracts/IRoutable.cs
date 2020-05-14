using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    
    public interface IRoutable
    {
        Task EnterAsync();
        Task LeaveAsync();
        Task<bool> CanEnterRouteAsync();
    }
}
