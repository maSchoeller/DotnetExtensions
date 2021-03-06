﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Universal.Abstracts
{
    public interface IRoutable
    {
        string Header { get; }

        void OnEnter();
        void OnLeave();
    }
}
